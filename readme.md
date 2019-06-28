# References
* https://www.mithunvp.com/angular-asp-net-mvc-5-angular-cli-visual-studio-2017/
* https://developer.okta.com/blog/2018/12/21/build-basic-web-app-with-mvc-angular

# Add angular 7 application to MVC
* npm install -g @angular/cli@7.3.9
* W folderze /Scripts:
    * ng new ChatApp -minimal
		
* Jeśli mamy dobrze zdefiniowany plik packages.json, to po wydaniu polecenia "npm install", nodejs zainstaluje pakiety w folderze "node_modules".
* Visual Studio 2017 sam powinien być w stanie tłumaczyć pliki *.ts na JavaScript automatycznie (ustawienia projektu).
* Można użyć browserify (post-build event) do utworzenia jednego pliku bundle.js, który będzie includowany w widoku.
* remove-node-modules (npm package)
*  ng new chat-web-client-ng7 --minimal --skip-tests --skip-git
* run the command `ng build –watch` to compile Angular code automatically. The compiler runs in background and listens for changes in code.