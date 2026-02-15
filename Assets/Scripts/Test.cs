using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

public class Test : MonoBehaviour
{
    static string _formURL = "https://docs.google.com/forms/d/e/1FAIpQLSd7055nRC71OqJzBFHombwjAR4i2my7YGhUEtZRf-QNLek6QQ/formResponse";
    static string _advQuestionID = "entry.1812678872";
    static string _saveStringID = "entry.2023510223";

    [ButtonMethod]
    public void TestForm()
    {
        Send();
    }

    public static async void Send()
    {
        var formData = new Dictionary<string, string>()
        {
            { _advQuestionID, "Advanced"},
            { _saveStringID, "saveString"},
        };

        try {
            using (HttpClient httpClient = new HttpClient()) {
                var content = new FormUrlEncodedContent(formData);

                HttpResponseMessage responseMessage = await httpClient.PostAsync(_formURL, content);

                if ((responseMessage.IsSuccessStatusCode)) print("Succsess");
                else print("Failed");
            }
        }
        catch (System.Exception e) {

            throw;
        }

    }
}
