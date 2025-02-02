Додайте в appsettings.json в секцию   

"GoogleKeys":{
      "ClientId": "836014924905-8h490t0c93guuq29aaves7p3kibo41l5.apps.googleusercontent.com",
    "ClientSecret": "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"
},
В файлі Program.cs розкоментуйте рядки
            //options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
            //options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;

та закоментуйте
            options.ClientId = cliendId;
            options.ClientSecret = clientSecret;
