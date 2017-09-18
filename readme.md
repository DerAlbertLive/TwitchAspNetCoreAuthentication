# ADC 2017, ASP.NET Core Security 18.09.2017
## Folien und Beispiel Code

In dem Beispiel Code sind die 4 ASP.NET Core 2.0 Anwendungen. Sowie ein auf IdentityServer 4 basierender OpenIdConnect Provider.

Für die Entwicklung  muss ASP.NET Core 2.0 installiert sein.
Visual Studio Code oder Visual Studio 2017.3 (15.3) hilft beim bearbeiten.

Folgender Beispeiel gibt es

* [Cookie Authentication](aspcore20/Cookie/)
* [OpenId Authentication](aspcore20/OpenId/)
* [Authorization](aspcore20/Authorization/)
* [DataProtection](aspcore20/DataProtection/)
 
sowie den [IdServer](idserver/IdServer)

Der IdServer ist aktuell noch gehostet unter https://idserveradc2017.azurewebsites.net/ und kann für die Benutzer *alice* und *bob* verwendet werden. Benutzername ist gleich Password. Wichtig für den Betrieb des OpenId Beispiel ist das dieser local auf *https://localhost:44387/* gehostet wird, sonst stimmen die Urls nicht.

Der IdServer kann jedoch auch selbst gehostet werden. z.B. fall eine Anpassung der ClientUrls notwendig ist.

DAS IST ALLES KEIEN PRODUCTION CODE! Zum Zwecke der Demostration ist vieles vereinfacht worden.


