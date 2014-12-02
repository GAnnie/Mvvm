// --------------------------------------
//  Unity Foundation
//  ConsoleSetup.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System.Collections;
using Foundation.Core;
using UnityEngine;

namespace Foundation.Examples
{
    [AddComponentMenu("Foundation/Examples/HttpTests")]
    public class HttpTests : MonoBehaviour
    {

        public string GetURL = "http://avariceonline.com/api/RestDemo/5";
        public string PostUrl = "http://avariceonline.com/api/RestDemo/5";
        public string DeleteURL = "http://avariceonline.com/api/RestDemo/5";
        public string PutURL = "http://avariceonline.com/api/RestDemo/5";

        public HttpServiceClient Client = new HttpServiceClient();

        protected void Awake()
        {
            Terminal.Add(new TerminalCommand
            {
                Label = "Get",
                Method = () => StartCoroutine(GetTest())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "Post",
                Method = () => StartCoroutine(PostTest())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "Put",
                Method = () => StartCoroutine(PutTest())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "Delete",
                Method = () => StartCoroutine(DeleteTest())
            });
        }

        private IEnumerator GetTest()
        {
            Debug.Log("GetTest : " + GetURL);
            var task = Client.GetAsync(GetURL);
            
            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Debug.LogError(task.Exception.Message);
            else
                Terminal.LogSuccess("OK :" + task.Result);
        }


        private IEnumerator PostTest()
        {
            Debug.Log("PostTest : " + PostUrl);
            var task = Client.PostAsync(GetURL, string.Empty);

            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Terminal.LogError(task.Exception.Message);
            else
            {
                Terminal.LogSuccess("OK :" + task.Result);
            }
        }

        private IEnumerator PutTest()
        {
            yield return 1;
            Debug.Log("PutTest : " + PutURL);
            //var task = Client.PutAsync(GetURL, string.Empty);

            //yield return StartCoroutine(task.WaitRoutine());

            //if (task.IsFaulted)
            //    Debug.LogError(task.Exception.Message);
            //else
            //{
            //    Terminal.LogSuccess("OK :" + task.Result);
            //}
        }

        private IEnumerator DeleteTest()
        {
            yield return 1;
            Debug.Log("DeleteTest : " + DeleteURL);
            //var task = Client.DeleteAsync(GetURL, string.Empty);

            //yield return StartCoroutine(task.WaitRoutine());

            //if (task.IsFaulted)
            //    Debug.LogError(task.Exception.Message);
            //else
            //{
            //    Terminal.LogSuccess("OK :" + task.Result);
            //}
        }
    }
}