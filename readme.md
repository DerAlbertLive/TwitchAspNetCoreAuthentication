# Code zum Live Stream https://www.twitch.tv/DerAlbertLive

Die Aufzeichung der passenden Sitzung ist auch auf YouTube https://www.youtube.com/watch?v=3bExQpA_eHo

## ASP.NET Core Authentication und Authorization Deep Dive
## Folien und Beispiel Code

In diesem Repository sind die 5 ASP.NET Core 2.2 Anwendungen. Sowie ein auf IdentityServer 4 basierender OpenIdConnect Provider.

Für die Entwicklung muss ASP.NET Core 2.2 installiert sein. http://dot.net

Visual Studio Code oder Visual Studio 2017.9 (15.9) oder neuer hilft beim bearbeiten.

Folgende Beispiele gibt es

* [Cookie Authentication](aspcore20/Cookie/)
* [OpenId Authentication](aspcore20/OpenId/)
* [Custom](aspcore20/Custom/)
* [Authorization](aspcore20/Authorization/)
* [DataProtection](aspcore20/DataProtection/)

sowie den [IdServer](idserver/IdServer). Und hier kann man die [Folien herunterladen](Twitch-ASP-Net-Core-AuthenticationDeepDive.pptx).

Der IdServerkann für die Benutzer *alice* und *bob* verwendet werden. Benutzername ist gleich Password. Wichtig für den Betrieb des OpenId Beispiel ist das dieser local auf *https://localhost:44387/* gehostet wird, sonst stimmen die Urls nicht.

Die Anwendungen selbst sollten alle ohne Problem direkt lokal ausgeführt werden können.

Der IdServer kann jedoch auch selbst gehostet werden. z.B. fall eine Anpassung der ClientUrls notwendig ist.

DAS IST ALLES KEIN PRODUCTION CODE! Zum Zwecke der Demostration ist vieles vereinfacht worden.



