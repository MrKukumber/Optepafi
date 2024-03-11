# Log

## 15.10.2023

- research elastickych grafov, riesenie, ktore vrcholy sa budu snapovat na objekt,
- najdeny software na konverziu ocad do jpg 
-ziadny     sposob     pre konverziu   s   omap   na obrazok   som   nenasiel,
- zacal     som    zvazovat generovanie       obrazku osobitne  mozno by som si mohol     spravit     ako zapoctak z javy

## 16.10.2023

hladam kniznice pre vektorovu grafiku - skusam rozne jazyky

- najdena python kniznica *drawsvg*, kotra v sebe zahrnuje vela ficur ako napriklad  prerusovane ciary, bezierove krivky
- java 2D - awt.Graphics a awt.Graphics2D kniznica, vcelku nanic podla vsetkeho, co som zatial nasiel, nezahrna bezierove krivky- update - nasiel som triedu CubicCurve2D, ktora zahrna bezierove krivky ale pride mi to strasne komplikovane, stale som nenasiel vychytavky ako dash line
- c# - trieda Graphcis - zahrna bezierove krivky, aj prerusovane ciary

asi zacnem predsa len najprv s touto grafickou castou, aby som si potom pri programovani spracovavania tej mapy do grafu mohol hned pozerat vysledky

## 18.10.2023

hladal som vyskove data pokryvajuce celu zem

- zistil som, ze v dnesnej dobe su najspolahlivejsie volne data z datasetu SRTM, s max 30 m rozlisenim a presnostou do 16 vyskovych metrov, povacsinou medzi 5-9 m
  - niektore zdroje tvrdia, ze je to 90 m ale tie su asi zastarale
  - maju svoje API, cez ktore sa da dotazovat na dataset
- alternativa su platene TessaDEM data, kotre su dostupne cez API-cko, za ktoreho dotazovanie sa plati, zato vyzera, ze maju rozlisenie 30m po celej zemeguli

## 19.10.2023

- premyslam nad tym, ze uzivatel si vzdy bude moct stiahnut alebo vymazat vyskove data pre danu krajinu
- nasledne oblasti, ktore mam stiahnute by sa mohli vykreslit na nejakej mapke v gui
- premyslal som, ci tuto manipulaciu s datami a ich požadovanie nebude lahsie robit v pythonu

- v ramci instalacie by som rad pouzil klasicky sposob s uzivatelskym rozhranim
  - uzivatel si bude moct vybrat kam chce aplikaciu ulozit
  - ci chce vytvorit odkaz na ploche a (odkaz v starte)
  - akym jazykom nanho ma aplikacia rozpravat
- toto by som si ale nechal az nakoniec, az bude spravene vsetko ostatne

- tiez mi napadlo, ze asi by som mohol aplikaciu lokalizovat, popripade globalizovat

## 20.10.2023

- zacal som sa pohravat s myslienkou, ci su winforms vhodnym gui pre moj vektorovo-graficky nastroj
- zistil, som ze existuje modernejsia kniznica WPF pre vytvaranie GUI-cok
- z tutorialu *https://www.youtube.com/watch?v=aB9Tgw2JZZI* som si zacal zistovat podrobnosti ohladom danej kniznice a vyzera to vcelku slubne

## 22.10.2023

- rozsirovanie mojho povedomia o WPF
- myslienka, ze by uzivatel si mohol vytvorit vlastny model na spracovanie vah mapy a ulozit si ho a potom si jednotlive modely znova nacitat, v developer rezimu
- jo a tiez teda pridat developer rezim...bude mat ovela vacsi pristup k praci s tymi vahami a bude prave moct vytvarat dane modely
- tiez by relevance feeback mohol byt v tychto modeloch dobrovolny....moze ho uzivatel vypnut a nastavit vsetky vahy konkretne

## 25.10.2023

- **stretnutie s veducim**
- odobril mi vacsinu mojich myslienok
- zacat by som mal pracovat na grafickom znazorneni omap-u a parseru ompau do grafu
- graficke znazornenie bude stacit velmi zakladne do tej aplikacie s tym ze spravim tam nastroj, ktory mi do daneho omap suboru ulozi aj line-y, ktore mi dany algoritmus vygeneruje ako najrychlejsie cesty a takyto omap si budem moct pozriet v nejakej inej aplikacii - bude to sluzit pre vytvaranie peknych obrazkov do bakalarky a pride mi to ako aj fajna ficura
- poslal mi bakalarku, ktora sa tykala podobneho projektu pre inspiraciu s mapovanim vyskovych dat do tej mapy a potazmo aj grafu
- ak chcem, aby dany nastroj bol rozsiritelny o dalsie mapove typy a vyhaldavacie algoritmy, musim sa velmi peclivo zamysleit na reprezentacii daneho grafu a interfacov, s ktorym pripadne rozsirenia budu pracovat
- vyskove data nebudu zachytavat vsetky nuancie terenu - vrstevnice zacnem zahrnovat az ked budem mat vela volneho casu
- graf skor tvorit tym sposobom ze nemam siet ktoru prilepujem na objekty ale siet sa mi postupne bude tvorit pridavanim objektov, s tym ze budem dovolovat podrozdelovanie hran, cize v niektorych miestach bude graf hustsi ako na inych miestach

## 26.10.2023

- najprv spravit graficke znazornenie toho omapu
- potom si v druom kroku vytvorim okno, v ktorom budem moct robit s tou grafovou reprezentaciou a popri tom vytvarat tu grafovu reprezentaciu
- napad na dizajn - mapa na pozadi a v popredi ovladacie prvky, ktore ju prekryvaju

## 8.11.2023

- srtm data budem stahovat pomocou python skriptu, v ktorom existuje kniznica prave pre tento ucel
  - https://mapbox.github.io/usgs/reference/api.html
- alebo pouzit priamo USGS EROS API - zlozitejsie
  - https://m2m.cr.usgs.gov/api/docs/json/

- vyuzit kniznicu *Alpinechough.Srtm* na spracovanie .hgt subor s vyskovymi datami - je open, len je treba dko zopakovat ten copyright co tam maju
  - https://github.com/alpinechough/Alpinechough.Srtm

## 21.11.2023

- zacal som vytvarat diagrmovy navrh aplikacie pre lepsie znazornenie mojich myslienok
- myslienka - jak algoritmi, formaty mapovych suborov alebo samotne modely budu podliehat *modelovym template-om*, ktore budu udavat, ake atributy ma graf na vrcholoch (hranach?), na ktorom sa potom bude hladat cesta
  - modely potom budu naplnat tieto parametry
  - algoritmy budu moct pracovat s tymito atributmi
  - pre kazdy mapovy format moze existovat niekolo konvertorov do roznych modelovych template-ov
- jendotlive modely potom pre implementaciu template-u mozu pouzivat vyskove data z databaze

## 24.11.2023

- moznost spustit viac relacii vyhladavania naraz, kazdu na zvlast vlakne

## 27.11.2023 23:40:00

- pokrok v navrhu aplikacie
- rozpisem zajtra

## 2.12.2023

- aplikacia rozdelena na frontend a backend
- frontend obsahuje gui a triedy priamo pracujuce s danym uzivatelskym rozhranim
- tieto triedy nasledne komunikuju s backend-om, ktory spracovava ich pokyny a vracia odpovede, ktore nasledne tieto triedy ukazuju v GUI
- kazdy ukon, ktory bude chciet uzivatel robit v aplikacii pripadne k tzv. session
- session bude v podste izolovane vlakno, ktore bude robit nejaku konkretnu cinnost
- nateraz budu len dva typy takychto session-ov a to jeden pre vyhaldavanie cesty a jeden pre vytvaranie instancii uzivatelskych modelov
- tym padom bude mozne mat otvorene viacero takychto session akehokolvek typu naraz....myslim si ze je to preveditelne
- kazdy session  bude mat svoj *controler*, ktory bude zabezpecovat komunikaciu s frontend-om a vracat mu pozadovane data
- kontroler nasledne komunikuje s jednotlivymi triedami, ktore zabezpecuju resource management, spracovavanie grafov, vykreslovanie grafov, spracovanvanie uzivatelskch modelov, hladanie ciest v algoritme, spracovanie vyskovych dat, ...
- tym padom kazdy kontroler je zavisly na jeho oknach a okna su zavisle na svojom kontroleru....teda session je vlastne definovany bud oknami a ich poziadavkami, ktore im kontroloer vyplna, alebo konstrukciou kontroleru, ktory ovlada svoje okna v gui a vydava cez ne uzivatelovi vystup...este sa treba rozhodnut, jak tam budu fungovat ta hierarchia

## 3.12.2023

- uvazujem vymenu WPF kniznice za multiplatformnu AvaloniaUI

## 26.2.2024

- znovuzoznamenie sa s problematikou, upravene poznamky trocha

## 27.2 2024

- ucenie sa Avalonia UI

## 28.2.2024

- upraveny navrh/diagram aplikacie
  - back end/front end navrh prerobeny na MVVM styl architektury
  - doplnenie jednotlyvych dat, ktore tecu v aplikacii
  - premyslel som si, akym sposobom bude fungovat prepojenie medzi modelom a model view-om - model view zbiera pokyny od view a zadava prislusne prikazy session controlerom ktory od modelu tahaju data a algoritmy pre zadovazenie pozadovanych vystupov, controlery si vsetko nechavaju u seba, len pocuvaju prikazy model view-vu, kedy maju zacat ktoru fazu riesenia
  - **uzavretie polemyzovania o tom, co chcem mat v aplikacii - nadalej uz nic nepridavam**
  - pridany subor s pociatkom programu, zatial je tam len nejaky nezmyselny kod...ale uz to pridam do gitu

## 1.3.2024

- zmena v navrhu:
  - grafiku si budu riesit entity z Windows ViewModels

## 11.3.2024

- nastudoval som si Avalonia UI kniznicu
- zacal som tvorit UI aplikacie