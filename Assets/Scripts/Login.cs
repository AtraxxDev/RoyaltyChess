using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Login : MonoBehaviour
{
    private string baseUrl = "http://localhost:8080/";

    public TMP_Text txtLogger;
    public TMP_InputField txtUser, txtPass;
    public Material mat;
    public GameObject panel;

    private int userId;

    [SerializeField] private BoardInitializer boardInitializer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //mat.color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void tryLogin()
    {
        string username = txtUser.text;
        string password = txtPass.text;

        if (username != "" && password != "")
        {
            StartCoroutine(testPost(username, password));
            panel.SetActive(false);
        }
        else
        {
            if (mat != null)
                mat.color = Color.red;

        }
    }

    IEnumerator testPost(string username, string password)
    {
        // Multi part
        /* 
        var formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username=asd&password=qwe"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        using var www = UnityWebRequest.Post(baseUrl + "user", formData);
        */

        // WWWForm
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        //form.AddBinaryData("fileUpload", bytes, "screenShot.png", "image/png");
        var www = UnityWebRequest.Post(baseUrl + "login", form);
        if (mat != null)
            mat.color = Color.cyan;


        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(json);

            txtLogger.text = $"Status: {response.status}\nID: {response.id}\nUser: {response.name}";
            userId = response.id;

            if (mat != null)
                mat.color = Color.green;

            StartCoroutine(GetProducts(userId));

        }
    }

    [ContextMenu("GetProducts")]
    public void InitializeGetProducts()
    {
        Debug.Log(userId);
        StartCoroutine(GetProducts(userId));
    }


    IEnumerator GetProducts(int userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("idUser", userId.ToString());  // igual que username/pass en testPost

        UnityWebRequest www = UnityWebRequest.Post(baseUrl + "products", form);
        if (mat != null)
            mat.color = Color.yellow;

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al obtener productos: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;

            ProductResponse[] productos = JsonHelper.FromJson<ProductResponse>(json);

            foreach (var prod in productos)
            {
                Debug.Log($"[DEBUG] Producto recibido: {prod.product}");
            }


            boardInitializer.InitializeWithProducts(productos);


            foreach (var prod in productos)
            {
                Debug.Log($"Producto: {prod.product} | Precio: {prod.price} | Descuento: {prod.discount_percentage}%");
            }

            if(mat!=null)
                mat.color = Color.green;
        }
    }
}
