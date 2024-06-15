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

## 10.5.2024

### refaktorizacia interfacov pre mapove reprezentacie

- uvedomil som si nespravnu logiku za svojou implementaciou implementacii/konstruktorov mapovych reprezentacii
- konstruktor mapovej reprezentacie konstruuje za pomoci vstupneho template-u a mapy, teda je proti logike, aby typove parametre pre tieto vstupy boli kovariantne, naopak by mali byt kontravariantne
- to sa ukazalo aj ako problem, ze keby sme chceli napriklad vytvorit mapovu reprezentaciu z potomka typoveho parametru konkretneho konstruktoru, tak by to neslo, lebo by sa v metodu create map v IDefinedFunctionalityMapRepreRep nenamatchoval dany konstruktor na typ daneho potomka
- preto som sa rozhodol toto zmenit a prerobil som architekturu tychto konstruktorov
- vytvoril som naviac tri dva nove interface-y IMapRepreImplementationInfo, IMapRepr(ElevDataDep)Implementation
  - IMapRepreImplementationInfo teraz reprezentuje zdroj templatu a mapy a tieto zdroje vracia a teda je uplen v poriadku, aby typove parametre tychto zdrojov boli covariantne....cize toto bude typ, ktory si reprezentant mapovej reprezentacie bude drzat ako mozne implementacie danej mapovej reprezentacie
  - IElevDataDependen/IndependentConstr nateraz ziskali ulohu vazne iba konstruovania mapovej reprezentacie, uz teraz nindikuju von, z coho mapovu reprezentaciu vytvaraju, preto mozu mat ako som spomenul vyssie kontravariantne genericke typy
  - IMapRepr(ElevDataDep)ImplementationRep je interface, ktory reprezentuje jak konstruktor, tak zdrojovy indikator, tento interface nasledne maju implementovat jednotlivi reprezentanti konkretnych implementacii
    - tito reprezentanti sa budu uchovavat ako zdrojove indikatory a nasledne sa budu matchovat pri vyrobe mapovych reprezentacii na konkretne mapove konstruktory
    - schvalne su vytvorene dva typove parametre pre mapu
      - jeden reprezentuje hlavnu mapu, ktorej format sa bude prezentovat v ramci IMapRepreImplementationInfo (kovariantne)
      - druhy reprezentuje pozadovaneho potomka mapy, ktory splnuje pozadovany interface konstruktoru (kontravariantne)

### pridanie interfaceov pre mapy

- pridany interface-y, ktore pridavaju funkcionalitu mapam
- konkretne pridavaju funkcionalitu ohladom ziskavania im korespondujucich vyskovych dat

### vytvoreny interface pre pracu s vyskovymi datmi

- vela toho nebolo, co spravit
- interface IElevDataSource reprezentuje zdroj vyskovych dat
- je potrebne aby dokazal stahovat a mazat stiahnute regiony
- region je definovy interface-om IRegion a kazdy datovy zdroj by mal regionu prisudit take datove celky, aby ho cely pokryl
- zaroven je potrebne 
  - aby datovy zdroj dokazal povedat, ci su vsetky data pre danu dotazovatelnu vstupnu mapu stiahnute
  - vedel vytvorit IElevData instanciu pre konkretnu dotazovatelnu vstupnu mapu
- IElevData instancie je mozne sa dotazovat na vyskove data, kazda instancia by mala obshaovat len tolko dat, kolko je pre korespondujucu mapu potrebne

- treba premysliet, ci nebude vhodne este skusit zabezpecit, aby sa nestiahnute potrebne vyskove data stiahli automaticky, ked to bude potrebne bez toho aby sa ulozili v pocitaci (potobny princip ako mapy cz)

## 16.5.2024

- idea - pri stahovani vyskovych dat je potrebne davat pozor na to, ze sa moze stahovat naraz viac regionov, ktore ked sa datovymi bunkami prekryvaju ich musi stahovat synchronizovane, teda aby sa niektore bunky nestiahli viac krat...to by sa dalo pri srtm datach vyriesit tak, ze pre kazdu datovu bunku budem mat instanciu, ktora mi bude hovorit, ci je dana bunka uz stiahnuta alebo nie a synchronizovane budem upravovat jej stav podla toho ci je stihanuta, nieje stiahnuta alebo je akurat stahovana

- pri vytvarani prvych ViewModelov a ModelViewov som sa rozhodol zacat pouzivat ViewModelove wraper typy pre datove typy, ktore su hodne toho aby uzreli svetlo sveta cez View
  - teda triedy ako IElevDataSource, IElevDataType, IRegion v pripade ElevConfigViewModelu, ale aj nasledne napriklad aj triedy IMapFormat\<ITemplate\>, ITemplate, ISearchingAlgorithm atd. dostanu svoje wrapper typy, ktore budu ponukat vlastnosti vnutornych datovych typov potrebne ku behu viewModelu a potazmo View-u 
  - zaroven tieto wrapper typy ako ostatne viewModely budu dedit od ViewModelBase co znamena, ze budu moct vyuzivat ficury Reactive UI
  - taktiez je tento princip povazovany za odporucany pri programovani MVVM aplikacie
  
### vytvoreny ElevConfigViewModel, ElevDataModelView a MainSettingsViewModel

#### MainSettingsViewModel

- v tomto momente su hlavne nastavenia velmi jednoduche, obsahuju len dve nastavitelne polozky
- v ramci tohto projektu ich asi uz nebudem viac rozsirovat ale viem si predstavit, ze by sa v nich dali nastavovat este podrobnejsie jednotlive modelove objekty ako napriklad nejake preferencie na vybery implementacii algoritmov, pouzite mapoveReprezentacie a ich implementacie, atd.
  - toto by vsak potrebovalo dodatocnu prerabku modelov, nakolko tie v tomto momente nedisponuju moznostou takychto nastaveni
- nateraz hlavne nastavenia teda obsahuju iba moznost zmeny lokalizacie aplikacie a moznost vyberu typu a potazmo zdroja vyskovych dat
- vyber zdroja vyskovych dat je prevedeny dialogovym sposobom, kedy po stlaceni tlacidla nastavujuceho vyskove data sa vytvori dialogove okno(v tomto pripade sa nevytvori okno ale len sa nastavenie vyskovych dat otvori ako view v hlavnom okne) v ktorom bude mozne zmenit typ(zdroj) vyskovych dat a stahovat jednotlive vyskove data alebo ich vymazavat a po odchodu z tohto okna sa nastaveny zdroj vyskovych dat vrati ako odpoved interakcie a uchova sa v hlavnych nastaveniach
- nesikovne sa v MainSettingsViewModelu odkazujeme na instanciu typu vnutry wrraper typu - to by sa nemalo diat, viewModel by mal vydiet vyhardne viewModelove wrrapery
  - MainSettingsViewModel vsak nema vlastny ModelView - preto sa hram s myslienkou, ze by sa mu mal jeden vytvorit a ParamsManagingModelView by sa mal posunut do Modelu ako ParamsManager
  - to by tiez pomohlo v tom, ze potom by sa ViewModely session-ov nemuseli handrkovat s ulozenymi MainSettingsParameters, ktore casto maju kripticku stringovu formu, ale mohli by sa opytat priamo MainSettingsModelView-u, ktory by zadovazil potrebne aktualne nastavenia

##### MainSettingsParameters

- bola vytvorena aj nova trieda pre uchovavanie parametrov z halvnych nastaveni
- uchovava posledne pouzitu Culture a meno typu posledne pouziteho typu vyskovych dat
- to ze sa uchovava meno typu posledne pouziteho typu vyskovych dat je mozne preto, lebo jednotlive vyskove zdroje a potazmo typy su singletony, ktore su prezentovane ElevDataManagerom a teda z mena typu by malo byt jasne, o ktoru instanciu zdroja potazmo typu vyskovcyh dat sa jedna
  - tato myslienka nalsedne bude platit pre vsetky typy s podobnou singletonovou myslienkou (ITemplate, IMapFormat<>, ISearchingAlgorithm, IUserModelType<>,...)
- tym ze sa vzdy jedna o jednu instanciu tychto parametrov v triede MainSettingsViewModel, ktorej sa iba nastavuju vlastnosti, nieje potrebne neustale volat metodu Set() ParamsManagingModelView-u s novymi parametrami, nakolko cez referenciu sa tam tie parametre budu konzistentne menit tiez

#### ElevConfigViewModel a ElevDataModelView

- nateraz sa ignoruje akakolvek hierarchia ci uz regionov alebo zdrojov/typov
  - vsetky regiony sa ukazuju vo wraper panelu, ci uz su to TopRegion-y, alebo SubRegion-y
  - ukazuju sa len datove typy, nie datove zdroje
- z predoslej odrazky je poznat, ze som jemne zmenil zdroje vyskovych dat - ponovom moze jeden zdroj drzat viacero typov vyskovych dat (napriklad USGS zdroj ma 3 alebo 1 secondArc-ove data)
- taktiez som s typmi vyskovych dat pridal aj nove interfacey, ktore informuju o tom, ci dany typ vyskovych dat potrebuje credentials na to, aby bolo mozne ho ziskat/stiahnut
  - pre tuto funkciu som pridal aj TextBoxy pre zadavanie mena a hesla
- stahovanie dat prebieha asynchronne a je mozne prerusit ho
- odstranovanie dat prebieha asynchronne taktiez ale je neprerusitelne

- TODO - je potrebne este vymysliet, ako sa bude uchovavat medzi behmi aplikacie informacia o tom, ktore regiony su stiahnute a ktore nie a kto to bude robit(najskor asi samotne zdroje, potazmo typy)
  - no jasne, lebo to je vlastne v ich rezii, aby sa stiahnutost regionove ukazovala spravne

## 18.5.2024

- TODO - je potrebne poriadne premysliet exitovanie apliacie a zatvaranie okien session-ov, neviem, ci to nenechat az na koniec, ked uz budem mat zvysnu funkcionalitu hotovu a teda budem vediet urcit, kedy uzivatelovi klast otazky, ci vazne chce opustit session/aplikaciu

### praca na PathFindingSettingsViewModel a PFSettingsModelView

- vytvoreny vyber templatu a vyhladavacieho algoritmu
- vytvoreny vyber suborov s mapou a s uzivatelsko-modelovou serializaciou
- vytvorena logika za otvaranim takychto suborov a ich nacitanim
- vytvorena logika za tym, ktory template, vyhladavaci algoritmus, mapovy format a typ uzivatelskeho modelu je kedy pouzitelny
  - pouzitelne template-y a vyhladavacie algoritmy upravovane v ListBoxe
  - pouzitelne mapove formaty a typy uzivatelskych modelov dosadene ako argumenty FilePicker-u, ktory zaisti, ze sa budu moct vybrat iba vhodne formty suborov
- vytvorena logika za tym, ze algoritmus sa moze vybrat iba vtedy, ked je vybrana mapa aj template a uzivatelsky model len vtedy, ked je vybrany template
- upravene vytvaranie map a uzivatelskych modelov - pridany cancellation token do tychto procesov - pokial uzivatel nepocka na vytvorenie mapy/uzivatelskeho modelu a vyberie nejaky iny subor, predosly vyber sa zahodi/cancell-ne a necha sa zpracovat novy vyber
- v konstruktore sa skontroluje, ci existuju nejake PathFindigParametre, ak ano, pokusi sa z nich nastavit template, mapu, vyhladavaci algoritmus a aj uzivatelsky model
  - po kazdom uspesnom pokracovani z nastaveni sa prepisu PathFindingParametre a dalsia session bude teda mat defaultne nastavene parametre prave na tieto aktualne
    - TODO - je este potrebne doriesti toto ukladanie parametrov

## 19.5.2024

- podla poslednych riadkov logu z 16.5.2024 v sekcii MainSettingsViewModel som sa rozhodol trocha prerobit viewModel a ModelView prave hlavnych nastaveni
  - vytvoril som MainSettingsModelView, ktory prebral zodpovednost na praci s ulozenymi parametrami
  - odstranil som zavislost na ElevDataModelView-vu, nakolko si z mena typu typu vyskovych dat uz sam bude dolovat samotny typ vyskovych dat
    - tato zmena plati aj pre ostatne ModelView-vy - musia si defaultny typ vyskovych dat vydolovat z parametrov sami
  - Z ParamsManagingModelView som vytvoril ParamsManager a presunul som ho medzi modely
- naviac bol vytvoreny MainWindowModelView, ktory nateraz nema prilis vela vyuzitie v sebe, ale posluzi dobre pri zatvarani aplikacie, kedy bude volat ParamsManager aby serializoval ulozene parametry
  - podobne ako pri SessionModelView-ovch bude drzat v sebe instancie na jednotlive ModeView-y korespondujuce MainViewModel-om

- mainSettingsModelView bude k dispozicii cez MainMenuViewModel, ktory bude tento modelView distribuovat do vsetkych hlavnych nastaveni potrebnych sessionov
  - teda napriklad session-y nebudu uz musiet ziskavat z MainSettingsParameters-ov aktualne pouzivany typ vyskovych dat

- pre konstrukciu mapy som sa rozhodol vytvorit dialogove okno, ktore bude davat informacie postupu tvorby mapy, popripade bude hlasat problemy pri vytvarani

## 22.5.2024

### implementacia MapRepreCreatingWindow + ViewModel + ModelView

- je rozdelena na tri casti
    1. Prerequisities checking - v tejto fazi sa skontroluju vsetky prerekvizity pre vytvorenie mapy
      - ak sa objavi nejaka zavada alebo sa skontroluju vsetky prerekvizity, prejde sa do 2. casti
    2. Problems Resolving - v tejto casti sa skontroluje, ci sa nasli nejake problemy v 1. casti
      - ak nie, hned sa spusti 3. cast
      - ak ano, pouzije sa spravne upozornenie pouzivatela a ponuknu sa mu moznosti, ako moze danu sytuaciu riesit (v tomto momente je to jedine vratenie sa do nastaveni ale v buducnosti by sa mohlo dat riesit aj inymi sposobmi)
        - nasledne ak uzivatel vyberie moznost pre opatovne skontrolovanie prerekvizit, spusti sa opat cast 1
    3. Map Repre Creating - v tejto casti sa uz len vytvara mapova reprezentacia
      - drzi sa predpokladu, ze vsetky prerekvizity pre vytvorenie mapy su uz v poriadku a teda nic nebrani tomu aby sa mapa vytvorila
      - zaroven v tejto casti sa objavi tlacidlo pre prerusenie vytvarania mapovej reprezentacie a progressBar reportujuci postup prace vytvarania reprezentacie
      - ak tretiu cast nic neprerusi, vrati true pre to ze uspecne vytvorila mapovu reprezentaciu, inak vrati false pre neuspech

### dialogove upozornenia uzivatelov pred zatvaranim okien

- vytvoril som YesNoDialogWindow + ViewModel pre upozornovanie uzivatelov pomocou jednoducheho dialogoveho okna
- pouzivanie je vsak trocha zlozitejsie nez by sa na prvy pohlad mohlo zdat... na to ay som nezastavil responzivitu UI a mohol s dialogovym oknom komunikovat je potrebne aby v OnClosing metode nic nebranilo UI aby bolo responzivne....lenze tym si aplikacia nepocka na zmenu Cancel vlastnosti eventoveho argumentu a teda zavrie aplikaciu bez toho aby uzivatel mal moznost to nejak zvratit
  - preto som vymyslel sposobob, ako tomuto chovaniu zabranit - defaultne sa nastavy vlastnost Cancel na true a zavola sa dialogove okno...az ked dialogove okno vrati svoju hodnotu sa na nu v OnClosing metode pozrieme a ak je nastavena na to aby bolo okno zavrete tak nastavime priznak ze sme sa uz raz pytali uzivatela na jeho nazor na zatvorenie na true a zavolame znova metodu Close ktora uz definitivne okno zavrie, pretoze uplne na zaciatku metody OnClosing sa pytame, ci sme sa uz raz uzivatela pytali na jeho nazor
  - v skratke zastavime prve zatvaranie aplikacie a az ked dostaneme odpoved od uzivatela tak sa rozhodneme ci znova zavolame zatvaraciu metodu alebo nie

### Uprava modelu

- Upravene interface pre mapove formaty - boli pridane dva dalsie, ktore reprezentuju este invarintu a contravariantnu variantu IMapFormat\<out IMap\>-u - dava to vacsi zmysel a taktiez nam to umoznuje dohladavat mapovy format pre jednotlive mapy
- Upravene interface-y mapovych reprezentacii - namiesto IFunctionalityDefiningMapRepre som tieto interface-y premenoval na IGraph a teda vsetko co malo nieco spolocne s IGraph interface-om som premenoval tak, aby to bolo prisposobene tomuto novemu pomenovaniu
  - pride mi to ako trefnejsi nazov, nakolko funkcionalita mapovej reprezentacie je vlastne graf ktorym ona sama je
  - zaroven to ponechava sematiku mapovych reprezentacii, ktore vanze reprezentuju nejakym sposobom mapu ale zaroven su grafmi, nad ktorymi dokaze algoritmus vyhladavat
  - interface-y pre implementacie mapovych reprezentacii som premenoval iba na implementacie, lebo v podstate budu implementovat ako mapovu reprezentaciu, tak graf...teda pre jednoduchost su to proste len implementacie

## 29.5.2024

### myslienky ku reprezentacii grafiky mapovych objektov

- vytvorim novu datovu strukturu ktora v sebe bude drzat mapove objekty - konkretne ich view modely v podstate
- tato datova struktura este bude drzat aj celkovu velkost mapy - aby sme mohli nastavit spravnu velkost canvasu
- jednotlive mapove objekty musia obsahovat ich poziciu a z-index(priorita vykreslovania), nasledne vsetky ostatne vlastnosti uz mozu byt definovane v potomkoch lubovolne - najlepsie tak aby sa to dobre parsovalo do datovych tamplatov
- nebudem si celu session musiet udrziavat nejaku dalsiu mapovu reprezentaciu pre vykreslenie v pamati - proces vytvorenia takej reprezentacie by mal byt o dost jednoduchsi ako mapovej reprezentacie pre vyhladavanie - preto nevadi ked sa to bude pocitat dva krat napriklad
  - alebo si popripade mozem podrzat ukazatel na kolekciu ktora bude obsahovat tie mapove objekty a mozem ju vyuzit viac krat - to uz sa budem moct rozhodnut pocas programovania...mala by to potom byt uz jednoducha uprava

- koordinacny system ktory budu pouzivat tieto mapove objetky este treba rozhodnut - bud *MapCoordinate* podla referencneho bodu mapy alebo *CanvasCoordinat* aby sa uz nemuseli prepocitavat vo view-u 
  - CanvasCoordinate znamena ze pozicia bude urcena tak aby sa mohla vlozit do Canvas.Left a Canvas.Bottom vlastnosti a tvary jednotlivych objektov budu relativne tejto pozicii aby sa tiez nemuseli prepocitavat

### myslienky ku renderovaniu mpovych objektov v canvase

- staci mat dataTemplates pre jednotlive typy objektov a ono aj ked budu v nejakej kolekcii ich predka IMapObject, budu si spravne nachadzat svoje templaty nakolko templaty funguju na Match a Build metode ktore pattern matchuju prichadzajuce objekty na ich DataType vlastnost.
- Canvas nastavim velkost podla velkosti ulozenej v datovej strukture
- Skusim pouzit *ScrollViewer* pre vyuzitie scroll barov na canvase a *ViewBox* pre moznost kvalitneho scale-ovania
- DataTemplate-y budem uskladnovat v extra ResourceDictionary a budem ich include-ovat do ListBoxu
- PointerPressedEvent pre zaznamenanie bodu

## 3.6.2024

- vytvaranie mechanizmu pre zobrzaovanie grafiky map
- co sa tyka modelu, vytvorena nova cast pre spracovavanie akejkolvek grafiky - GraphicsManager
- ponuka metody pre spracovanie grafik pre mapy a cesty
- robi to tak ze naplna predostrety kolektor gprafickymi objektmi aby sa mohlo v realnom case pozorovat pribudajuce grapficke objekty
  - z tohto dovodu musi grapficky manager taktiez zprostredkovat minimalne a maximalne suradnice map, aby sa dopredu mohlo zistit, s akymi rozmermi mapy musime pocitat

- v ramci model viewu vytvorene triedy pre konverziu grafickych mapovych objektov vytvorenych modelom na ich view modely kore sa daju pouzivat vo view-e 
- dalej kolektory ktore sa davaju grapfickemu mamageru, ktory ich naplnuje mapovymi objektmi
  - kolektor je objekt kotry mal len metodu Add a teda nechava do seba pridavat novo vytvorene graficke objekty. Tieto ojekty vklada do predlozenej kolekcie ktoru uz nasledne moze observovat modelView alebo dokonca View (ak sa v kolektore zabezpeci konverzia na viewModely)
  - tento zvlastny princip je vytvoreny koli tomu, aby sme mohli v relanom case pozorovat vytvaranie grafiky....graficke objekty su agregovane z mapy asynchronne....tym ze kolektor pridava do kolekcie spusta eventy oznamujuce prichod dalsich objektov

- vo viewModelu vytovrene viewModely pre jednotlive graficke objekty

- vo View-u pridana dataTemplate-y pre viewModely grafickych objektov
- vsetky data template-y sa pridavaju do Application a teda su dostupne pre celu aplikaciu
  - su pridavane v code-behind nakolko je potrebne do DataTemplates vlastonosti Application objektu popridavat datatemplate-y z resource dictionaries
  - dataTemplate-y su totiz rozdelene pre prehladnost v samostatnych suboroch (resorce-dictionaries) pre jednotlive mapove formaty a teda je potrebne ich nacitat do aplikacie
  - toto bol asi jediny sposob akym som dokazal separovat dataTemplaty do zvlast suboru
- pridany konvertor z mikrometrov na Desktop-Independent Pixels...viewModely si totiz drzia pozicie a parametere objektov v mikrometroch na mape....treba preto konvertovat tieto hodnoty na DIP aby mohli byt pouzite vo View-e

## 6.6.2024

- posun v myslienke uskaldnovania grafickych objektov a ich konvertovanie na ich viewModely
- dovodom je zlozitost vykreslovania objektov v avaloniovom canvase
- myslienka je ze predsa len si ukaldat graficke objekty, nielen ich priamo konvertovat na viewModely a tym publishovat celu graficku reprezentaciu mapy naraz
- nasledne na viewModely konvertovat len tie graficke objekty, ktore je potrebne pre zobrazenie v danej chvili
  - vo viewModeloch si predsa len pamatat aj graficky objekt, z ktoreho bol vytvoreny (teda moze graficky objekt dedit od DataViewModel-u aby mal aj overridenute equals a tak)
  - graficke objekty totiz oproti uz samotnej avaloniovej grafike nezaberaju takmer ziadne miesto a teda sa nemusim bat si ich drzat pocas celej doby session-u....v podstate ked sa to tak zoberie tak z jednej mapy vytvorim dve reprzentacie, ktore si proste budem drzat po celu dobu behu sessionu - mapovu reprezentaciu a mapovu grafiku

-myslienku s kolektormi by som ale ponechal, nech stale graficky agregatori v modelu ich pouzivaju na skladovanie nimi vytvorenych grafickych objektov....

- teda bude potrebne vitovrit v ModelView-e mechanizmus, ktory bude podporovat logiku zobrazovania grafickych objektov a konverziu na viewModely

## 10.6.2024

- implementovna nova logika z logu 6.6.2024
- zatial vsak nebude potrebne vykreslovat iba nejake casti mapy, nakolko vzdy bude zobrazena cela mapa na canvase...casti sa budu hodit az pri relevance feedbacku napriklad

- co sa vykonnosti tyce, nahradil som vkladanie objektov do ObervableCollection za SourceList na ktory je bindnuta ReadOnly OC a tento pristup ovela zlepsil vykonnost zobrazovania....sice stale okno trocha zalagovane ale na druhu stranu uz je responzivne, nezamrza 
  - sice uplne nerozumiem z akeho dovodu sa vykonnost zleplsila....predsa len som si myslel ze problem bol s vykreslovanim objektov, nie s ich vkladanim do kolekcie...vyzera to tak ze mozno source list dokaze nejakym rozumnejsim sposobom pridavat elementy do kolekcie a teda UI nieje zahltene privalom novych objektov
- mozem sa teda bez vycitok svädomia pustit do path finding okna s tym ze k jeho dokonceniu uz mam dokoncene takmer vsetky zavislosti....teda az na vykreslovanie cesty a implementacie smileyFace algoritmu


### myslienky ku path findingu, Path a celkovo behu algortimu

- mapova reprezentacia/graf by mala byt schopna providnut pre jednotlive vrcholy ich MapCoordinate poziciu, Path si postavi uz kazdy algoritmus podla seba
- alogrithm kazdy typ reportu by mal mat svoj viewModel a taktiez View...to znamena v modelView-u path findingu zastavit Progress instanciu a podat managerovy pre vyhladavanie inu instanciu Progress triedy a na nej subscirbovat a nou reportovane udaje konvertovat na viewModely a az tie poslat spat ViewModelu ktory ich subscribe-uje...popravde rovnaka vec by sa mala spravit aj pre createion progress....aj ked tam je len jedna konkretna trieda ktora reprezetnuje report progressu vytvarania mapovej reprezentacie - ale pre koretknost MVVMMV by sa to spravit malo

## 15.6.2024

### mechanizmus pre reportovanie stavov vyhladavania a najdenych ciest

- vytvoril som noveho manager-a ktory dokaze agregovat reporty o stavoch vyhladavani a o najdenych cestach
- implementoval som novy typ konstruktu, tzv. SubManager
  - tieto konstrukty su urcene pre pouzitie priamo z modelov, nie z modelView-ov
  - vyznacuju sa tym, ze obsahuju genericke parametre, ktore by sa z modelView-u tazko dosadzovali
  - pre modely by vsak nemal byt problem s nimi pracovat a usetri mnoho volani visitorov a pattern matchingu
- vytvoril som subManagera pre ReportManager-a a aj pre GraphicsManager-a

#### reporty

- novy konstrukt
- moze obsahovat grafiku v podobe GraphicsSource-u a reportovane informacie ci uz o stavu vyhladavania alebo o najdenej ceste
  - path report by mal vzdy obsahovat grafiku, ktora zobrazi najdenu cestu
- pri vytvarani reportov sa zoberie vstupna cesta alebo stav vyhladavania a zodpovedajuci user model a nasledne sa z cesty/stavu vyhladavania agreguje report za pomoci uzivatelskeho modelu
- uzivatelsky model nieje nijak testovany na to, ci dokaze dorucit pozadovane sluzby
  - pokial to zvladne, tak dana informacia sa do reportu vlozi, ak nie tak sa vynecha
  - algoritmus teoreticky moze vyzadovat nejaku vlastnost od uzivatelskeho modelu o ktorej vie, ze nasledne bude potrebna pri agregacii reportu...napriklad nutnost toho, aby uzivatelsky model vedel vratit poziciu ulozenu vo vrcholovm atribute (ku vyzadovaniu funkcionalit od uzivatelskych modelov algoritmom citaj dalej o refactoringu myslienky user modelov, ...)
- pre jednotlive reporty su nasledne vytvorene viewModely ktore su predane view-u
- jemne rozdiely medzi pathReport-mi a SearchingReport-mi v sposobe ich distribucie z modelov:
  - PathReport je vytvarany z algoritmom vratenej cesty - za jeho aggregovanie zodpoveda modelView
  - SearchinReport je aggregovany z SearchingState-u algoritmom za pouzitia ReportSubManager-u a uz agregovnany report je predavany modelView-u na spracovanie
- podobne ako pre graficke objetky, je vytvoreny system konvertorov reportov na viewModely
  - musi vzdy existovat nejaky konvertor pre dany typ reportu, ktory ho prevedie na odpovedajuci viewModel
  - danemu ViewModelu nasledne musi prisluchat odpovedajuci DataTemplate vo View-e, ktory ho spravne zobrazi

### refactoring myslienky user modelov, mapovych reprezentacii a vyhladavacich algoritmov

- doslo mi, ze je nezmyselne ocakavat od mapovej reprezentacie, ze by mala byt schopna vypocitat spravne napriklad heuristiku pre A\* algoritmus ... ona totiz nemoze mat ponatia o tom, aku vahu ma taka heuristika vygenerovat aby bola *prijatelna*
- uvedomil som si, ze na vypocty roznych hodnot z atributov vrcholov a hran sa musi vyhradne pouzivat uzivatelsky model, ktory dokaze z atributov vypocitat vysledne hodnoty
- tym padom som sa rozhodol pre refactoring uzivatelskych modelov ktore dosledkom su mimo ine:
  - uzivatelske modely budu podobne ako grafy implementovat rozne funkcionality
  - algoritmy si musi definovat, ake funkcionality uzivatelskych modelov vyzaduju pre svoj chod
    - to znamena novo vytvorenu zavislost algoritmu na funkcionalite uzivatelskeho modelu
  - odstranenie funkcionalit grafov, ktore sa tykaju vypoctov na uzlovych/hranovych atributoch
    - ponechane tie, ktore definuju strukturu grafu, teda strukturu vrcholov a hran

- zaroven prislo ku jemnej zmene reprezentacie zavisloti uzivatelskeho modelu na konkretnom template-e
  - vyjadrenie zavyslosti som zosilnil v tom zmysle, ze som odstranil negenericky IUserModel a jeho funkciu presuvania nahradil IUserModel<out TTemplate>
  - rozmyslel som si poriadne ze TTemplate logicky moze byt covariantny...popisuje totiz iba nieco, k comu je uzivatelsky model bindnuty, nie s akymi vrcholovymi a hranovymi atributmi vie pracovat
  - to s akymi atributmi vie zaobchadzat nasledne definuju contravariantne typy v interface-u IComputingUserModel<out TTemplate, in TVertexAttributes, in TEdgeAttributes>
- sice to niektore casti kodu jemne zneprehladnilo, myslim si ale ze myslienkou to ale tento nedostatok prebije
- taktiez som ako do IUserModelType, tak do IUserModel pridal novu vlastnost ktora vracia bidnuty template - ulahci to pracu v tom, ze uz nebude potrebne predavat zbitocne naviac template aby sa zistilo, ake typy atributov su pouzite v uzivatelskom modelu
