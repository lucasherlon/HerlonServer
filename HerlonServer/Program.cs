using System.Net;

class RequestHandler
{
    static void Main(string[] args)
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/");

        listener.Start();
        Console.WriteLine("Listening for requests...");

        while (true)
        {
            HttpListenerContext context = listener.GetContext();
            ProcessRequest(context);
        }
    }

    static void ErrorHandler(HttpListenerResponse response)
    {
        string htmlFilePath = "C:\\Users\\samsung\\source\\repos\\HerlonServer\\HerlonServer\\pages\\error_route.html";
        string htmlContent = File.ReadAllText(htmlFilePath);

        response.StatusCode = (int)HttpStatusCode.NotFound;
        response.ContentType = "text/html";
        response.ContentLength64 = htmlContent.Length;

        using (Stream output = response.OutputStream)
        using (StreamWriter writer = new StreamWriter(output))
        {
            writer.Write(htmlContent);
        }

    }

    static void ProcessRequest(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        string path = request.Url.LocalPath;
        string slicePath = path.Substring(1);
        string htmlFilePath = $"C:\\Users\\samsung\\source\\repos\\HerlonServer\\HerlonServer\\pages\\{slicePath}";
        Console.WriteLine("Request for path: " + path);

        if (path == "/" )
        { 
            string htmlContent = File.ReadAllText("C:\\Users\\samsung\\source\\repos\\HerlonServer\\HerlonServer\\pages\\index.html");

            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "text/html";
            response.ContentLength64 = htmlContent.Length;

            using (Stream output = response.OutputStream)
            using (StreamWriter writer = new StreamWriter(output))
            {
                writer.Write(htmlContent);
            }

            Console.WriteLine($"Status: {response.StatusCode}");
            
        }
        else if (File.Exists(htmlFilePath))
        {
            string htmlContent = File.ReadAllText(htmlFilePath);

            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "text/html";
            response.ContentLength64 = htmlContent.Length;

            using (Stream output = response.OutputStream)
            using (StreamWriter writer = new StreamWriter(output))
            {
                writer.Write(htmlContent);
            }

            Console.WriteLine($"Status: {response.StatusCode}");
        }
        else
        {
            ErrorHandler(response);
            Console.WriteLine($"Status: { response.StatusCode}");
            Console.WriteLine("The route requested does not exist.");
        }

        response.Close();
    }

}


