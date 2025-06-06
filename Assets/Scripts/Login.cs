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

    void Start()
    {
        // ✅ Ocultar panel si ya hay sesión guardada
        if (PlayerPrefs.HasKey("userId"))
        {
            userId = PlayerPrefs.GetInt("userId");
            Debug.Log($"Sesión encontrada. userId = {userId}");
            panel.SetActive(false);
            StartCoroutine(GetProducts(userId));
        }
        else
        {
            panel.SetActive(true); // mostrar login
        }
    }

    public void tryLogin()
    {
        string username = txtUser.text;
        string password = txtPass.text;

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            StartCoroutine(testPost(username, password));
        }
        else
        {
            if (mat != null)
                mat.color = Color.red;
        }
    }

    IEnumerator testPost(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

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

            userId = response.id;

            // ✅ Guardar sesión
            PlayerPrefs.SetInt("userId", userId);
            PlayerPrefs.SetString("username", username);
            PlayerPrefs.SetString("name", response.name);
            PlayerPrefs.Save();

            txtLogger.text = $"Status: {response.status}\nID: {response.id}\nUser: {response.name}";

            if (mat != null)
                mat.color = Color.green;

            panel.SetActive(false); // ✅ ocultar panel tras login exitoso

            StartCoroutine(GetProducts(userId));
        }
    }

    [ContextMenu("GetProducts")]
    public void InitializeGetProducts()
    {
        int storedUserId = PlayerPrefs.GetInt("userId", -1);
        if (storedUserId != -1)
        {
            Debug.Log($"Recuperado de PlayerPrefs: userId = {storedUserId}");
            StartCoroutine(GetProducts(storedUserId));
        }
        else
        {
            Debug.LogWarning("No hay userId guardado en PlayerPrefs");
        }
    }

    IEnumerator GetProducts(int userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("idUser", userId.ToString());

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

            if (mat != null)
                mat.color = Color.green;
        }
    }
}
