using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;

public class LoginController : MonoBehaviour
{
    [Header("API")]
    public string loginUrl = "https://10.14.255.40:8000/api/login";

    [Header("Inputs")]
    public TMP_InputField correoInput;
    public TMP_InputField passwordInput;

    [Header("UI")]
    public TextMeshProUGUI mensajeText;

    void Start()
    {
        if (PlayerPrefs.GetInt("UsuarioLogueado", 0) == 1)
        {
            SceneManager.LoadScene("2_Menu");
        }

        if (mensajeText != null)
        {
            mensajeText.text = "";
        }
    }

    public void IniciarSesion()
    {
        if (correoInput == null || passwordInput == null)
        {
            Debug.Log("Faltan inputs por conectar.");
            return;
        }

        string correo = correoInput.text;
        string password = passwordInput.text;

        if (correo == "" || password == "")
        {
            if (mensajeText != null)
            {
                mensajeText.text = "Ingresa correo y contraseña";
            }

            return;
        }

        StartCoroutine(LoginCoroutine(correo, password));
    }

    IEnumerator LoginCoroutine(string correo, string password)
    {
        if (mensajeText != null)
        {
            mensajeText.text = "Iniciando sesión...";
        }

        string json = "{";
        json += "\"email\":\"" + correo + "\",";
        json += "\"password\":\"" + password + "\"";
        json += "}";

        UnityWebRequest web = new UnityWebRequest(loginUrl, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        web.uploadHandler = new UploadHandlerRaw(bodyRaw);
        web.downloadHandler = new DownloadHandlerBuffer();
        web.SetRequestHeader("Content-Type", "application/json");

        
        web.certificateHandler = new ForceAcceptAll();

        yield return web.SendWebRequest();

        if (web.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error login: " + web.error);
            Debug.Log("Respuesta API: " + web.downloadHandler.text);

            if (mensajeText != null)
            {
                mensajeText.text = "Usuario o contraseña incorrectos";
            }   

            yield break;
        }

        Debug.Log("Respuesta login: " + web.downloadHandler.text);

        bool loginCorrecto = RevisarLogin(web.downloadHandler.text);

        if (loginCorrecto)
        {
            PlayerPrefs.SetInt("UsuarioLogueado", 1);
            PlayerPrefs.Save();

            SceneManager.LoadScene("2_Menu");
        }
        else
        {
            if (mensajeText != null)
            {
                mensajeText.text = "Correo o contraseña incorrectos";
            }
        }
    }

    bool RevisarLogin(string respuesta)
    {
        try
        {
            JObject data = JObject.Parse(respuesta);

            int idUsuario = 0;

            if (data["IDUsuario"] != null)
            {
                int.TryParse(data["IDUsuario"].ToString(), out idUsuario);
            }
            else if (data["IdUsuario"] != null)
            {
                int.TryParse(data["IdUsuario"].ToString(), out idUsuario);
            }
            else if (data["idUsuario"] != null)
            {
                int.TryParse(data["idUsuario"].ToString(), out idUsuario);
            }

            if (idUsuario > 0)
            {
                GuardarDatosUsuario(data);
                return true;
            }
        }
        catch
        {
            Debug.Log("No se pudo leer la respuesta del login.");
        }

        return false;
    }

    void GuardarDatosUsuario(JObject data)
    {
        int idUsuario = 1;

        if (data["IDUsuario"] != null)
        {
            int.TryParse(data["IDUsuario"].ToString(), out idUsuario);
        }
        else if (data["IdUsuario"] != null)
        {
            int.TryParse(data["IdUsuario"].ToString(), out idUsuario);
        }
        else if (data["idUsuario"] != null)
        {
            int.TryParse(data["idUsuario"].ToString(), out idUsuario);
        }
        else if (data["usuario"] != null)
        {
            JObject usuario = (JObject)data["usuario"];

            if (usuario["IDUsuario"] != null)
            {
                int.TryParse(usuario["IDUsuario"].ToString(), out idUsuario);
            }
            else if (usuario["IdUsuario"] != null)
            {
                int.TryParse(usuario["IdUsuario"].ToString(), out idUsuario);
            }
            else if (usuario["idUsuario"] != null)
            {
                int.TryParse(usuario["idUsuario"].ToString(), out idUsuario);
            }
        }

        PlayerPrefs.SetInt("IDUsuario", idUsuario);

        if (data["nombre"] != null)
        {
            PlayerPrefs.SetString("NombreUsuario", data["nombre"].ToString());
        }

        Debug.Log("Usuario guardado con ID: " + idUsuario);
    }
}
