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
  - teraz sa zaoberam oknom pre konfiguraciu vyskovych dat
  - treba este doriesit sposob vyberu regionov

## 12.3.2024

- doriesil som, ako bude fungovat elevation configuration
  - kazdy region ma svoj button a po jeho stlaceni sa zobrazi dany region pod "mapou" a moze sa stiahnut/vymazat
- teraz reisim ako spravim main menu view

## 17.3.2024

- pokracovanie v praci na aplikacii
  - vytvarama prepojenia a bindings view-u a viewmodel-u
  - pre kazdy view vytvoreny viewModel a pridany do gitu

## 20.3.2024

- znovu rozmyslanie architektury
  - ci vyhladavaci algoritmus nechat specificky pre kazdu session alebo ho nasavovat globalne pre celu aplikaciu
  - spravit ViewModel tak, aby jednotlive ViewModely prisluchajuce jednotlivym View-s neboli na sebe zavisle...teda aby si medzi sebou nemuseli predavat data
    - predavanie dat by malo prebiehat nasledne iba v session controleroch, ktore potom dane data sprostredkuvaju ViewModel-om
    - teda nemalo by sa stat, ze si viewModely predavaju napriklad mapu ako je to teraz
  - to vsak nastoluje otazku, kolko prace vlastne viewModely budu robit...ci budu napriklad sami pozadovat graficke spracovania map a ciest

## 21.3.2024

- pokracovanie v rozmyslani architektury
  - vyhladavaci algoritmus specificky pre kazdy druh sessionu ako ostatne parametry
  - upravenie MVVM modelu aby sedel viac na terajsiu architekturu aplikacie
    - v diagrame je zachovany aj povodny navrh, aj novy, viacej prisposobeny MVVM architekture
  - okrem ViewModelu budem mat aj tzv ModelView - ten bude odpovedat povodnym session controlerom s tym ze bude mat stale povodnu funkciu spracovania dat pre jednotlive session-y, ale nebude "kontrolovat" priebeh jednotlyvych session-ov
  - ViewModel:
    - stara sa hlavne o view, vie co sa ma kedy zobrazovat, ako ma fungovat ovladanie aplikacie, obsahuje vedomu logiku aplikacie
    - jednotlive viewModel-y by mali byt co najnezavislejsie, data prijimat vyhradne od ModelView-u, kory je shcopny im vsetky dorucit
  -ModelView:
    - stara sa hlavne o model, spracovava data, obsahuje postupy, ktore su potrebne na vygenerovanie pozadovanych dat ViewModel-om,
    - jednotlive ModelView-vy si medzi sebou data predavaju, aby nedochadzalo ku neziaducim opakovaniam vypoctov, a nasledne ich ponukaju ViewModel-om
    - zaroven poznaju logiku spracovania dat, vedia co je potrebne spravit v akom poradi, aby boli schopne zadovazit pozadovany vystup
  - v posledne uvedenej vlastnosti sa budu ModelView a ViewModel trocha byt...je potrebne vzdy urcit, kto bude zodpovedat za danu logicku cinnost aplikacie

## 22.3.2024

- dokoncena architektura pre MVVM+MV model

## 23.3.2024

- doporucena kniznica Rastom, na spracovanie geospatila dat - *doplnit* - mozno mi to nejakym sposobom pomoze na seba nalepit mapu s vyskovymi datami...

## 26.3.2024

- napady z vcerajsej konzultacie
  - dynamicky nechavat vyhodnocovat vahy hran pocas vyhladavania cesty a casche-ovat tieto hodnoty
  - spracovavat v mape aj tratove symboly a potom vyhladavat na tejto trati vs interaktivne vyhladavanie medzi dvomi bodmi
  - zahustovanie grafu pocas hladania cesty (zlozite, mozno nie vhodne pri tom interaktivnom vyhladavani)
    - teda nechat spracovanie grafu zavisle na algoritmu...algoritmus si urci, ako chce mat predspracovany graf kym sa na nom spusti
    - problem zavislosti algoritmu na template-u a mapovom formatu

## 3.4.2024

- pri nacitani mapy naparsujem mapu do objektovej reprezentacie a nasledne tato reprezentacia sa bude vyuzivat ako mapa z ktorej sa budu vytvarat dalsie konstrukty
  - tympto sposobom zamadzim prilis dlhemu uzamknutiu mapoveho suboru / problemy so zmenou/odstranenim mapoveho suboru pocas tvorenia dalsich konstruktov
  - tym ze vytvorim reprezentaciu, usetrim aj mnoho miesta, ktore by bolo potrebne pri nacitani xml mapy

## 5.4.2024

- zacal som vytvarat triedy pre konstrukciu modelu
  - vzdy existuje nejaka staticka trieda s ktorou sa da komunikovat a ziskavat od nej potrebne informacie
  - nasledne existuju interface-y pre tzv agentov jednotlivych datovych struktur, ktori identifikuju jednotlive datove struktury a zaroven vytvaraju dane datove struktury, ktore su taktiez potomkovia jednotlivych interface-ov...
    - je potrebne este pouvzovat o nazvoch "agentskych" tried a interfacov, ci to agent dava zmysel a zaroven sa zamysliet nad nejakym zapuzdrenim napriklad este...ze napriklad jednotlive struktury dokazeme vytvarat iba pomocou agentov

## 24.4.2024

opozdely log z programovania projetku

### co sa tyka modelu:

- jednotlive datove komponenty v procese maju svojich reprezentantov, tj. mapovy format, template, zastupca reprezentacie mapy a algoritmus
- tito reprezentanti su singletony a su ponukane uzivatelovi, aby si z nich vybral vhodnu kombinaciu
- reprezentant ma vacsinou potom schopnost vytvorit zastupovanu mapu, mapovu reprezentaciu ci algoritmovy executor, s ktorym sa nadalej pracuje
- zastupca povacsinou obsahuje genericky parameter, ktory reprezentuje datovy typ, ktory zastupuje
- template je specialy reprezentant, reprezentuje iba sam seba a v podstate sluzi hlavne ako typ pre genericke parametre jednotlivych objektov, ktore ked napriklad chcu spolupracovat, tak musia suhlasit prave templatovym generickym parametrom
- taktiez este kazda datova komponenta ma aj svojho manager-a, co byva povacsinou staticka trieda, ktora usnadnuje pracu s danym typom dat

---

- konkretne ku jednotlivym datovym objektom:

#### IMapFormat je zastupcom nejakej IMapy

- IMap je objektova reprezentacia nejakeho mapoveho suboru, mala by byt vytvoritelna v linearnom case, bez zloziteho spracovania...varianta je napriklad iba ulozit text suboru (not ideal)

#### ITemplate je zastupcom sameho seba

- vyjadruje to, ake atributy by mal graf v jeho mene obsahovat
- taktiez sa pre konkretny template vytvaraju uzivatelske modely, ktore z jeho atributov dokazu spocitat vahu jednotlivych hran grafu

#### IMapRepreRepresentativ zastupuje nejaku IMapRepresentation

- komplikovanejsi koncept ako predosle dva
- kazdy reprezentativ obsahuje kolekciu konstruktorov, ktore reprezentuju schopnost pre dany template a mapu skonstruovat mapovu reprezentaciu
- mapova reprezentacia, ktoru reprezentuje nejaky reprezentativ je potom iba interface, ktory je implementovany pre jednotlive konkretne kombinacie map a template-ov, navonok vsak uz vystupuje iba ako ona sama reprezentacia
- konstrukcia je v podstate pred okolnym svetom schovana, cez MapRepresentationManager-a ani niesu vidiet jednotlive konstruktory, ci implementacie interfacov mapovych reprezentacii. Jedine co je vidiet su pouzitelne kombinacie map a templatov pre vytvorenie danej reprezentacie

#### ISearchAlgorithm bude podobne ako ITemplate zastupovat sameho seba

- este neodmyslene na 100%
- bude dorucovat svoj vlastny executor, v ktorom sa nasledne bude moct dany algoritmus spustat a ziskavat za jeho pomoci najrychlejsie cesty
- 

---

- Naskytla sa otazka, ci nespravit aj ModelView genericky, tym usetrit mnoho trapenia s visitor patternom a castovanim vsobecne. Rozhodol som sa vsak neist touto strastiplnou cestou, nakolko sice by to ulahcilo pracu s Modelom, horror by vsak nastal v prepojeni negenerickeho ViewModel-u s generickym ModelView-om.
- Cize cely ModelView zostane negenericky, teda pri presune jednotlivych dat bude treba v kazdom modely znova castovat/view-pattern-ovat a type-check-ovat vsetky data

## 26.4.2024

### co sa tyka modelu

- upraveny genericky visitor pattern, vytvoreny, tak aby bol co najgeneralickejsi
- koli tomu bolo treba prerobit manager-ov zo statickych tried na singletony, aby mohli implementovat IVisitor interface-y

#### UserModel, IVertex, IOrientedEdge, Attributes, Coords

- IVertex\<TTemplate\> a IOrientedEdge\<TTemplate\> budu dva obecne koncepty pre reprezentaciu vyhladavacieho grafu
- kazdy Vertex bude sprostredkovavat mnozinu orientovanych hran z neho veducich, nasledne metodu/vlastnost pre ziskanie a nastavenie vahy pre jednotlive hrany
  - sprostredkovanie tymto metodovym sposobom zabezpeci, ze si kazda mapova reprezentacia bude moct vrcholy implementovat podla vlastneho uvazenia
  - teoreticky, nakolko by dane metody nemali trvat dlho mozne nahradit vlastnostami, ktore si kazda mapova reprezentacia bude moct implementovat sama
- OrientedEdge bude obsahovat zasa dva vrcholy, pociatocny a koncovy
- kazdy vrchol a hrana su genericke podla nejakeho template-u a nasledne v sebe nesu atributy daneho template-u
- samozrejme mapove reprezentacie mozu implementovat dodatocne interface-y, ktore zabezpecuju dodatocne vlastnosti vrcholov/hran, implementaciu tychto interface-ov mozu nasledne jednotlive algoritmy vyzadovat

- UserModel\<TTemplate\> bude obsahovat metodu, ktora dostane hranu a vrati jej vahu, nic viac, nic menej
- bude musiet byt schopny vypocitat vahu pre akukolvek moznu kombinaciu atributov, ktore template umoznuje

- Coords bude jednotny system koordinantov v aplikacii
- jednotlive templaty si mozu pozicie zachovavat v akymkolvek sposobom, avsak musia byt preveditelne z a na typ Coords
- v tychto jednotkach bude komunikavat ViewModel s ModelViewom a potazmo Modelom
- taktiez vysledna cesta, vracana algoritmom bude obsahovat koordinanty jednotlivych vrcholov cesty v Coords systeme

#### SearchAlgorithm

- kazde spustenie algoritmu si bude uzamikat danu mapovu reprezentaciu, aby s nou niekto iny nezacal nahodou pracovat
- lagoritmy budu mat dva typy spustenia, bud klasicky cez metodu `Path[][] Execute((Coord,Coord)[] Model[])`, ktora spusti trat zlozenu z viacerych postupov na vsetkych vlozenych modeloch
  - alebo pomocou vyziadaneho executoru, ktoremu sa poda konkretny model a nasledne na nom nezavysle mozno pytat trate 
  - executor si taktiez zamyka mapovo reprezentaciu pre seba, dokym nieje dispose-nuty, vtedy uvolnuje mapovu reprezentaciu
  - tymto sposobom sa zaruci, ze executor nemusi znova a znova prepocitavat uz raz prejdene oblasti
  - preto je potrebne s nim narabat aj opatrne, nakolko pri dinamicky generovanych mapovych reprezentaciach moze byt problem s velkostou vygenerovaneho grafu
  
- ci uz po skonceni metody Execute alebo po dispose-nuti executoru, mapova reprezentacia by mala byt navratena do konzistenteneho stavu pred uzamknutim
- algoritmus si toto zkonzistentnenie vyziada a mapova reprezentacia sa donho musi vediet sama dostat

## 4.5.2024

### co sa tyka modelu

#### dokoncene koncepty pre reprezentaciu reprezentacii map

- rozhodol som sa mapove reprezentacie spravit genericke iba cez vertex a edge atributy, nie cez samotne verteces a edges
  - nebolo mozne poriadne ich implementovat genericke aj cez vrcholy a hrany, nakolko:
    1. uz teraz je to riadna motanica
    2. interface-y funkcionalit beru zodpovednost za definovanie constrainov na hrany a vrcholy, ktore musia dane mapove reprezentacie splnat
    3. hold celkovo nemusia byt mapove reprezentacie ani algoritmy genericke cez vrch.+hran., nakolko ci uz bude vrchol reprezentovany hodnotovym ci referencnym typom, budu si to algoritmy vyzadovat prave cez interface-y funkcionalit a mapove reprezentacie si zasa mozu v pripade referencnych typov vytvarat ich potomky alebo v pripade hodnotovych typov si k danym strukturam ukaldat este aj dodatocne informacie naviac pomimo tychto struktur, (popripade sa vytvori struktura pre vrchol a hranu, ktore budu obsahovat naviac referenciu na typ "ine", kam si bude moct reprezentacia ulozit co len bude chciet)
- reprezentanti mapovych reprezentacii su nadalej dvaja, jeden co reprezentuje mapovu reprezentaciu ako koncept a druhy, ktory ju reprezentuje ako pouzitelnu reprezentaciu na prehladavanie
  - po dlhom snazeni sa mi podarilo dat vsetko dokopy tak, aby to fungovalo, bohuzial este niesom stale prilis spokojeny s runtime-ovym checkom vo "funkciovom reprezentatovi" na spravnost mapoveje reprezentacie, tento check by bolo najlepsie aby sa robil za prekladu, avsak v tom pripade by musel constructor mat o parameter naviac a to prave danu "funkcionalnu mapovu reprezentaciu", aby vedel overit ze vytvarana realna mapova reprezentacia je vazne typu danej funkcionalnej
- zmenil som aj to, ze mapove reprezentacie uz niesu genericke cez template, nakolko su genericke cez vrch.+hran. atributy, preto nieje potrebne, aby boli genericke aj cez template
  - to aby atributy vychadzali z jednoho template-u zabezpecuje vytvaranie mapovych reprezentacii a typova parametrizacia ich konstruktorov
- "funkcioveho reprezentanta" je mozne ziskat generickou metodou z klasickeho reprezentanta, toto posluzi napriklad na kontrolu, ci reprezentant reprezentuje pouzitelnu mapovu reprezentaciu v danom algoritme

#### par myslienok ku algoritmu

- neprisiel som na sposobom, ako algoritmus donutit, aby sa zhodovala kontrola reprezentantov mapovych reprezentacii na pouzitelnost danych reprezentacii s kontrolou uz dodanej mapovej reprezentacie na pouzitie pre vyhladavanie 
  - preto bude musiet implementacia tieto kontroly implmentovat sama a snad spravne
  - este nad tym popremyslam
- algoritmy budu moct mat viacero svojich implementacii, teda bude existovat moznost pre vacsie pokrytie mapovych reprezentacii

## 6.5.2024

- interface pre vyhladavaci algoritmus a pracu s nim funguje presne tak ako bolo popisane 26.4.
- zacinam s tvorbou ParamsSerializeru

### ParamsSerializer

- musi sa rozmyslat spolocne s MainParamsModelViewom
- moja myslienka je taka, ze jednoducho si ktokolveke bude moct definovat svoju strukturu s parametrami, ktora bude dedit od nejakeho IParam interface-u, aby sa tam nestrkali bohvieake objekty
- nasledne MainParamsModelView bude obsahovat genericke metody, ktore ako genericky parameter budu obsahovat typ danej parametrovej struktury a budu nastavovat/vracat parametry v/z slovniku
- potom pri ukoncovani aplikacie sa da pokym MainParamsModelView-vu, aby zavolal serializer, ktory vsetky tieto parametry ulozi tak, aby bolo z nazvu jasne, aky typ je v danom subore ulozeny
- MainParamsModelView bude ziskavat ulozene parametry z predoslieho behu aplikacie lenivo - teda pokial sa v jeho slovniku dane parametre nenachadzaju, poziada serializer, aby sa pozrel ci pre dany typ nema serializovane data
  - ak ma, vrati ich MaiParamsModelView-u, ten si ich ulozi do slovnika a vrati ich
  - ak nema, MainParamsModelView vrati null a poznaci si do slovnika, ze take parametry nema ulozene(napriklad null-om), aby sa vyhol opakovanemu dotazovania Serailizeru

- teda ParamsSerializer by mal byt schopny serializovat slovnik parametrov(paralelne napriklad aj), popripade jednotlive parametre a sprostredkovavat postupne jednotlive deserializovane objekty pomocou nejakej generickej metody