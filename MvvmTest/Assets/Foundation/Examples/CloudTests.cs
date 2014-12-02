using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Foundation.Cloud;
using Foundation.Core;
using LITJson;
using UnityEngine;

namespace Foundation.Examples
{
    [AddComponentMenu("Foundation/Examples/CloudTests")]
    public class CloudTests : MonoBehaviour
    {
        [StorageTable("Test")]
        public class TestObject
        {
            [StorageIdentity]
            public string Id { get; set; }

            [StorageScore]
            public float Score { get; set; }

            public DateTime Date { get; set; }
            public int Number { get; set; }
            public string String { get; set; }
        }

        public string UserName = "Demo";
        public string Email = "demo@unity3dfoundation.com";
        public string Password = "password1";

        public string UserName2 = "Demo2";
        public string Email2 = "demo2@unity3dfoundation.com";
        public string Password2 = "password2";

        public string[] MessagingChannels = { "Main" };

        public string Token = string.Empty;

        public string Session;

        protected CloudAccount Client;
        protected CloudStorage Storage;

        protected void Awake()
        {
            Storage = Injector.GetFirst<CloudStorage>();
            Client = CloudAccount.Instance;
            Debug.Log(Client.IsAuthenticated + " " + Client.UserEmail);

            Terminal.Add(new TerminalCommand
            {
                Label = "SignUp",
                Method = () => StartCoroutine(SignUpAsync())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "SignIn",
                Method = () => StartCoroutine(SignInAsync())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "SignOut",
                Method = () => StartCoroutine(SignOutAsync())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "ReAuthorize",
                Method = () => StartCoroutine(ReAuthorizeAsync())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "Token",
                Method = () => StartCoroutine(TokenAsync())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "Save",
                Method = () => StartCoroutine(UpdateAsync())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "TokenRevert",
                Method = () => StartCoroutine(TokenRevertAsync())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "UpdateRevert",
                Method = () => StartCoroutine(UpdateRevertAsync())
            });


            Terminal.Add(new TerminalCommand
            {
                Label = "StorageCreate",
                Method = () => StartCoroutine(StorageCreateAsync())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "StorageUpdate",
                Method = () => StartCoroutine(StorageUpdateAsync())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "StorageGet",
                Method = () => StartCoroutine(StorageGetAsync())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "StorageDelete",
                Method = () => StartCoroutine(StorageDeleteAsync())
            });


            Terminal.Add(new TerminalCommand
            {
                Label = "StorageCreate12",
                Method = () => StartCoroutine(StorageCreate12Async())
            });
            Terminal.Add(new TerminalCommand
            {
                Label = "StorageQuery",
                Method = () => StartCoroutine(StorageQueryAsync())
            });
        }

        private IEnumerator SignUpAsync()
        {
            Debug.Log("SignUpAsync : ");

            var task = Client.SignUp(UserName, Email, Password);

            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Terminal.LogError(task.Exception.Message);
            else
            {
                Terminal.LogSuccess("OK :" + Client.Session);
                Session = Client.Session;
            }
        }

        private IEnumerator SignInAsync()
        {
            Debug.Log("SignInAsync");
            var task = Client.SignIn(Email, Password);

            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Terminal.LogError(task.Exception.Message);
            else
            {
                Terminal.LogSuccess("OK :" + Client.Session);
                Session = Client.Session;
            }
        }

        private IEnumerator SignOutAsync()
        {
            Debug.Log("SignOutAsync");
            Client.SignOut();

            Terminal.LogSuccess("OK :" + Client.Session);

            yield return 1;
        }

        private IEnumerator ReAuthorizeAsync()
        {
            Debug.Log("Authorize");

            var c = new Dictionary<string, string[]>();

            foreach (var messagingChannel in MessagingChannels)
            {
                c.Add(messagingChannel, new []{"r","w","p"});
            }

            var task = Client.Authorize(c);

            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Terminal.LogError(task.Exception.Message);
            else
            {
                Terminal.LogSuccess("OK :" + Client.Session);
                Session = Client.Session;
            }

        }

        private IEnumerator TokenAsync()
        {
            Debug.Log("TokenAsync");
            var task = Client.Token(Email);

            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Terminal.LogError(task.Exception.Message);
            else
            {
                Token = task.Result.Token;
                Terminal.LogSuccess("OK :" + task.Result.Token);
            }
        }

        private IEnumerator UpdateAsync()
        {
            Debug.Log("UpdateAsync");
            var task = Client.Update(UserName, Token, Email2, Password2);

            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Terminal.LogError(task.Exception.Message);
            else
            {
                Terminal.LogSuccess("OK :" + Client.Session);
            }
        }

        private IEnumerator TokenRevertAsync()
        {
            Debug.Log("TokenRevertAsync");
            var task = Client.Token(Email2);

            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Terminal.LogError(task.Exception.Message);
            else
            {
                Token = task.Result.Token;
                Terminal.LogSuccess("OK :" + task.Result.Token);
            }
        }

        private IEnumerator UpdateRevertAsync()
        {
            Debug.Log("UpdateRevertAsync");
            var task = Client.Update(UserName, Token, Email, Password);

            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Terminal.LogError(task.Exception.Message);
            else
            {
                Terminal.LogSuccess("OK :" + Client.Session);
            }
        }

        //

        private List<TestObject> Objects = new List<TestObject>();

        private IEnumerator StorageCreateAsync()
        {

            var entity = new TestObject
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.UtcNow,
                Number = UnityEngine.Random.Range(0, 100),
                Score = UnityEngine.Random.Range(0, 100),
                String = RandomString(UnityEngine.Random.Range(5, 10))
            };

            Debug.Log("StorageCreateAsync");
            var task = Storage.Create(entity, StorageACL.User, UserName);

            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Terminal.LogError(task.Exception.Message);
            else
            {
                Terminal.LogSuccess("OK");
                Objects.Add(entity);
                Terminal.Log(JsonMapper.ToJson(entity));
            }
        }

        private IEnumerator StorageGetAsync()
        {
            Debug.Log("StorageGetAsync");
            var task = Storage.Get<TestObject>(Objects.Random().Id);

            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Debug.LogError(task.Exception.Message);
            else
            {
                Terminal.LogSuccess("OK");
                Terminal.Log(JsonMapper.ToJson(task.Result));
            }
        }

        private IEnumerator StorageUpdateAsync()
        {
            var entity = Objects.Random();

            entity.Date = DateTime.UtcNow;
            entity.Number = UnityEngine.Random.Range(0, 100);
            entity.Score = UnityEngine.Random.Range(0, 100);
            entity.String = RandomString(UnityEngine.Random.Range(5, 10));


            Debug.Log("StorageUpdateAsync");
            var task = Storage.Update(entity);

            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Terminal.LogError(task.Exception.Message);
            else
            {
                Terminal.LogSuccess("OK");
            }
        }

        private IEnumerator StorageDeleteAsync()
        {
            var entity = Objects.Random();

            Debug.Log("StorageDeleteAsync");
            var task = Storage.Delete(entity);

            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Terminal.LogError(task.Exception.Message);
            else
            {
                Terminal.LogSuccess("OK");
                Objects.Remove(entity);
            }
        }

        //

        private IEnumerator StorageCreate12Async()
        {
            for (int i = 1;i < 12;i++)
            {
                var entity = new TestObject
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = new DateTime(2014, i, 1),
                    Number = i,
                    Score = i,
                    String = "Query"
                };
                
                Debug.Log("StorageCreate10Async");
                var task = Storage.Create(entity, StorageACL.User, UserName);

                yield return StartCoroutine(task.WaitRoutine());

                if (task.IsFaulted)
                    Terminal.LogError(task.Exception.Message);
                else
                {
                    Terminal.LogSuccess("OK");
                    Objects.Add(entity);
                    Terminal.Log(JsonMapper.ToJson(entity));
                }

            }

        }

        private IEnumerator StorageQueryAsync()
        {
            Debug.Log("StorageQueryAsync");

            var q = new StorageQuery<TestObject>().WhereEquals("String", "Query");

            var option = UnityEngine.Random.Range(0,4);

            switch (option)
            {
                case 0:
                    break;
                case 1:
                    q = q.WhereGreaterThan("Number", UnityEngine.Random.Range(1, 6));
                    break;
                case 2:
                    q = q.WhereLessThan("Number", UnityEngine.Random.Range(6, 12));
                    break;
                case 3:
                    q = q.WhereGreaterThan("Date", new DateTime(2014, UnityEngine.Random.Range(1,6), 1));
                    break;
                case 4:
                    q = q.WhereLessThan("Date", new DateTime(2014, UnityEngine.Random.Range(6, 12), 1));
                    break;
            }

            q = q.Skip(UnityEngine.Random.Range(0, 6)).Take(UnityEngine.Random.Range(1, 12));

            var task = Storage.Get(q);

            yield return StartCoroutine(task.WaitRoutine());

            if (task.IsFaulted)
                Debug.LogError(task.Exception.Message);
            else
            {
                Terminal.LogSuccess("OK");
                Terminal.Log(JsonMapper.ToJson(task.Result));
            }
        }

        //

        /// <summary>
        /// Randoms the string.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;

            for (int i = 0;i < size;i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * UnityEngine.Random.value + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
