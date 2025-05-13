# Myslienky

- riadit sa proste standardnymi hodnotami z ISOM-u
- vyuzit reinforcement learningu, na urcenie konkretnych hodnot zpomalenia behu jednotlivymi porastami
  - tie su standartne urcene v isome do intervalov a teda by bolo fajn nechat uzivatela nech si vyberie hodnotu z danych intervalov taku, aby popisovala co najlepsie jeho predstavu idealneho postupu
- vytvorit converter medzi geo suradnicovym systemom a canvas suradnicami, kde vstupom je velkost canvas

## reprezentacia mapy grafom

- reprezentovat mapu pruznym grafom
- postupne pridavame jednotlive aspekty mapy a podla potreby vytvarame husty graf
- zaroven nastavim indikatory vrcholov siete na take aby zodpovedali danemu pridanemu mapovemu objektu
- vrcholy na hranach objektov zafixujeme a nenechame nimi hybat okrem pripadu, ze su prekryte inym objektom, ktory ma vyssi level kreslenia ako ten predchadzajuci
  - v tom pripade odblokujeme vrchol a nechame ho, nech sa znova moze snapnut na hranu
- objekty budeme pridavat na zaklade ich levelu kreslenia
- ak dostaneme neprekonatelny polygon, nemusime davat vrcholy do jeho obsahu
- vrchol bude bud zafixovany alebo nie, ak nebude zafixovany, bude v najmenej energeticky narocnej polohe

### parametre, ktore treba nastavit pri tvorbe grafu

- vzdialenost medzi pridavanymi vrcholmi
- pruznost grafu
- spomalovanie jednotlivych kombinacii porastov
- pomer medzi spomalenim previsenim a mapovymi prvkami

## relevance feedback

- v modelu môžeme nastaviť, či chceme využiť relevance feedback a v akom intervale sa ma dany parameter nastavovany relevance feedbackom nastavovat
- mal by obecne fungovat pre akykolvek tameplate
- proste sa bude pozerat, ktore hodnoty este treba donastavit, najde vyskyty v grafe tychto hodnot a v danom regione vyberie nahodne dva body a nechat medzi tymito dvomi bodmi prebehnut relevance feedback a donastavit parametre

## elevation data configuration

- pre kazdy zdroj vyskovych dat
  - pytat od neho regiony, ktore poskytuje a su mozne ku stiahnutiu
  - pre kazdy region jeden button, po ktoreho zmacknuti sa dany region zobrazi pod "mapou" s moznostou stiahnutia/vymazania
- napad pre znazorneneie mapy...vyuzit nejake geo json data k nakresleniu mapy

## searching algorithm configuration

- otazka, ci mat nastaveny jeden jediny vyhladavaci algoritmus pre celu aplikaciu
  - nastavovany z hlavnych nastaveni
  - a ked sa prenastavy pocas beziacich sessions, tak ci sa maju preorientovat na novy vyhladavaci algoritmus alebo maju pokracovat s povodnym

## sessions settings

- bud defaultne parametre natvrdo zadane alebo vzdy upravene na posledne pouzite
  - dalo by sa spravit tak, ze budem mat property pre indikaciu, ktora z tychto dvoch moznosti sa pouziva
  - nasledne bude property pre natvrdo nastavenie parametrov...ked sa zavola setter tejto property, automaticky sa indikator nastavi na tvrde zadavanie parametrov
  - automaticke upravovanie defaultnych parametrov na posledne použité sa bude vykonavat bez toho, aby sa menila property uvedena vyssie a tym padom sa ani nezmeni dany indikator...dalo by sa zaistit vnutornym privatnym upravovanim ulozenych parametrov
  - pre znovu nastartovanie automatickeho upravovania sa len upravy dana indikatorova property
  - pokial sa indikatorova property nastavy na tvrde zadavanie parametrov, zachova sa posledna pouzita konfiguracia parametrov a bude sa pouzivat uz stale ako defaultna

## specifikacia

### co nezabudnut

- zmienit, ze sa budeme drzat ISOM 2017
- preco mam taku architekturu MVVMMV aku mam, co ktore casti robia a spravuju, ako su navzajom oddelene, co vidia a co zase nie 
- pre modelView skryva niektore vlastnosti pred ViewModelom pomocou inner typov definovanych v Session ModelView-e
- Preco vyuzivam viewmodely pre vsetky data ktore je potrebne nejakym sposobom ukazat vo view-e, preco nepouzivam priamo tie data
- preco maju jednotlive modely architekturu aku maju, 
  - preco su tam nejaki reprezentanti,
  - preco algoritmus ponuka aj jeho executor,
  - aky vyznam maju mapove reprezentacie vs aky vyznam maju grafy,
  - vyznam nepredavania celeho grafu algoritmu aby sa mohol dynamicky generovat
  - ...
- preco som sa rozhodol pre drzanie kolekcie mapovych objektov aj napriec modelViewvmi, preco nevytvaram tuto kolekciu vzdy od zaciatku, pro a proti, preco nevytvaram tuto kolekci vzdy znova relativne k pozadovanemu vystrihu mapy (narocne, alebo ked uz mam raz vytovrenu celu kolekciu tak sa mi ju neoplati zahadzovat a vytvarat nejaku mensiu ked ju takci tak budem potrebovat celu pri path findingu???)
- co je potrebne spravit na pridanie specifickeho modelu (mapoveho formatu, templatu, algoritmu, ...)
- ze nech grafy implementuju co najviac interface-ov co mozu, tym si zvyssia usabilitu v algoritmoch, nemusia sa bat ze by koli tomu ze implementuju nieco navyse stratili vykonnost, ak to po nich nieje pozadovane...proste mozu vytovrit naviac aj implementaciu pre algoritmy, ktore nevyzaduju tolko funkcionalit a teda pre ne zlepsit svoju efektivitu (napriklad vytvorit vlastny vrchol ktory bude mat shchopnost pamatat si predchodcu a jeden, ktory nebude...potom tym algoritmom ktore nevyzaduju aby si vrcholy pamatali predchodov bude predavat jednoduchsi vrchol s mensou pamatovou zlozitostou) 
- co treba vsetko vytvorit a spravit ked chcem pridat jednotlive konstrukty, teda template-y, mapy, mapove reprezentacie/grafy, implementacie map repre/grafov, userModels, algoritmy, implementacie algoritmov, Paths, SearchingReports, ...

## Co tam chcem mat

- GUI
  - hlavne menu
    - prechod do nastaveni, model-creating window, map choosing window
  - nastavenia
    - nastavenie lokalizacie
    - uzivatelske rozhranie pre stahovanie srtm dat
      - vyber zdroja vyskovych dat
      - stahovanie podla statov/vyber jednoho daneho chunk-u na mape
    - vyber defaultneho algoritmu (zavisi na modelovom template-e)
  - model-creating window
    - vytvaranie instancie uzivatelskeho modelu
      - nastavenie parametrov jednotlivych atributov instancie uzivatelskeho modelu daneho modeloveho tameplate-u
      - kolko prikladov sa naraz ukaze pri relevance feedback-u/kolko krat spravit relevance feedback
    - ulozenie modelu
  - model-creating setting window
    - upravit uz existujuci
      - vyber uzivatelskeho modelu
    - vytvorit novy
      - vyber modeloveho template-u
      - vyber vyhladavacieho algoritmu
  - path fiding setting window
    - vyber modeloveho template-u
    - vyber mapoveho suboru(format suboru zavisli na template-u)
    - vyber modelu (v zavislosti na modelovom template-u)
    - stiahnutie potrebnych vyskovych dat (s povolenim a ak sa da), nevyzadovat, ak niesu potrebne
    - moznost vratit sa do hlavneho menu
    - prechod do relevance feedback okna
    - multiThreading, moznost zapinania viacerych vyhladavani a kazdu relaciu pustit na zvlast vlakne, obmedzit nejakym sposobom pocet spustenych relacii, potazmo vlakien
  - relevance feeback window
    - pocet okien s moznostami podla udaju v modelu
    - navrat do map choosing okna
  - path-finding window
    - vykreslenie cesty na jednoduchoej mape, ktoru si nakreslim sam a vytvorenie suboru, ktory budem moct otvorti v oomapperu a ukaze sa mi tam aj ta moja cesta

- graficke vykreslovanie mapovych formatov
  - **implementacia** pre *omap* format
- interface pre Graf
  - **implementacia** pruzny graf ktory sa tvori postupnym pridavanim elementov pre *omap* format a *Orienteering* tameplate
- interface pre hladanie najrychlejsej cesty v grafe
  - necha algoritmu cely graf, nech si s nim robi co chce
  - bude vyzadovat nejaky standartny vystup z algoritmu, ktory nasledne pre vykreslovaciemu modulu, aby ho vykreslil
  - **implementacia** jednoduchy *A\**(popripade nieco  komplexnejsie ak vyjde cas) pre *Orienteering* tameplate
- interaktivne vykreslovanie najdenych ciest v mape
  - vykreslovanie vo vytvorenej grafickej reprezentacii mapy v aplikacii
  - generovanie suboru s vykreslenou trasou (nepovynne)
  - **impelementacia** pre *omap* format
- vytvaranie uzivatelskych rozhrani/modelov(parametrov pre graf)
  - moznost pevneho nastavenia parametrov alebo intervalov pre relevance feeback
- interface pre spracovane vyskove data
  - spracovanie vyskovych dat
  - requestovanie vyskovych dat
    - uzivatelske rozhranie pre spravu vyskovych dat
  - **implementacia** pre *srtm* data
- relevance feedback mechanizmus
  - mal by obecne fungovat pre akykolvek tameplate
- instalacny balicek
- lokalizacia

## Co budem ukladat

- vyskove data (pre kazdy zdroj zvlast)
- uzivatelske modely
- mapove formaty s pridanymi najdenymi cestami
- ukladame pre opatovne spustenie aplikacie
  - posledne pouzity algoritmus, tameplate, model, posledny zdroj vyskovych dat, mapa - parametry
  - posledne pouzity zdroj vyskovych dat, stiahnute regiony

## TODO do buducna:

- nechat nech view vykresluje iba objekty ktore sa v danej chvili su vidiet na obrazovke...neviemsice uplne teraz preco ale proste mohlo by to pomoct nejakym sposobom mozno ku zlepseniu vykonu
- poriadne spravit lokalizaciu
- zlepsit graficku stranku aplikacie
- podpora pluginov
- zlepsit uzivatelsku privetivost
- pridat spravne spracovavanie dier v polygonoch v omap file-och - nateraz diery v polygonoch ignorujem - upravit aj vykreslovanie map aj vytvaranie mapovych reprezentacii + pri vytvarani chainov spracovavat nove vytvorene diery vzniknute specifickym zapletenim polygonu objektu (neprocesovat novo vzniknute chainy oddelene ale naraz)
- v tejto chvili by malo byt mozne vo vytvarani mapovej reprezentacie pripajat chain-u na vsetky blizke vrcholy grafu, nie len na tie, ktorym bola pretata hrana - malo by sa este overit - razantne zlepsenia hladania ciest v mestach
- spracovat vinohrady a jednosmernu vegetaciu
- upravit parsovanie mapy - pokial by na vstupe bol obrovitansky objekt, ktory moze zmrazit program, odignorovat ho - dajme tomu, ze by mal viac ako 100_000 suradnic - je mozne, ze zmrazenie dojde uz pri citani daneho objektu zo suboru, ale to neviem ako by som riesil...mozem pozriet, ci ta kniznica na parsovanie xml to nejak handluje
- test toho, ci uz nahodou nejake optepafi niekde nebezi

## DONE

- nech kazda mapa moze po svojom znazornovat trat zadanu uzivatelom...nejako tak ze sa grafickemu managerovi predaju pozicie trate a mapa pre ktoru ma byt grafika vytvorena a on to za pomocou aggregatoru vytvori...v podstate vsetky graficke objekty budu dost podobne...az potom datatemplate vo view-u podla typu objektu urci jak sa to nakresli
- vacsiu custom-izaciu toho, ktora implementacia sa kedy pouzije, moznost viacerych implementacii algoritmov pre rovnake kombinacie mapovych reprezentacii s uzivatelskymi modelmi a uzivatel bude moct vybrat nejakym sposobom preferovanu implementaciu...to iste pre mapove reprezentacie a ich implementacie - implementovane pomocou konfiguracii kotre mozu upravovat beh algoritmu
- spracovanie trate zo vstupneho suboru
- nechat nech mapovu reprezentaciu moze implementovat viac grafov, ktore budu mat rozne vlastnosti a teda bude mozne otestovat viac moznosti vo vyhladavcom algoritme, ci aspon jedna z grafovych reprezentacii je pouzitelna. Vsetky grafove reprezentacie pritom budu dodrzovat podstatu mapovej reprezentacie.
- naprogramovat stahovanie srtm dat z USGS a upravit chovanie vyhladavania na zaklade stiahnutych vyskovych dat

## Myslienky do buducna

- ked spravim moznost definovat pre mapovu reprezentaciu viac grafovych rerpezetnacii, spravit aj grafy, v ktorych sa pohybujeme vo vrcholoch nedeterministicky, potom na to pustit nejakeho UI agenta, ktory bude pocitat s tym, ze sa nemusi nachadzat tam kde chcel ist, zacnu byt symboly dolezite aj koli orientacii, obzervovanie prostredia bude taktiez obmedzene

## Stare myslienky

- spravit nejaky research toho, aky je rozdiel medzi behanim v roznych porastoch
- nemetricke MDS
- pouzit nieco, co mi bude simulovat rozhodovaci proces pretekara...ze ked vidim ze to inakadial nejde, tak tomu proste trocha zvysimm preferenciu
- alebo vazne to robit nejak iterativne s tym ze vstup bude len porovnanie tych typov porastu a terenu

Předpokládá se možnost parametrizace vyhledávácích algoritmů např. údaji o rychlosti postupu v různých typech terénu spolu s interaktívnym donastavením parametrov, ako aj možnost porovnání nalezených tras pro různé přístupy k vyhledávání (různé algoritmy, parametry pro rychlosti, uvažování výškových dat apod.)

- reprezentovat mapu sietovym grafom (napriklad trojuholnikovim alebo akymkolvek inym druhom) a nechat ho ako tak pruzny v tom smere, ze ked pridam nejaky mapovy objekt, tak sa mi na jeho hrany pritiahnu najblizsie body k tymto hranam
- zaroven sa mi zmenia indikatory hran v bodoch siete na take aby zodpovedali danemu pridanemu mapovemu objektu
- taketo pritiahnute vrcholy na hranu objektu zafixujeme a nenechame nimi hybat okrem pripadu, ze su prekryte inym objektom, ktory ma vyssi level kreslenia ako ten predchadzajuci
  - v tom pripade odblokujeme vrchol a nechame ho, nech sa znova moze snapnut na hranu
- objekty budeme pridavat na zaklade ich levelu kreslenia
- ak dostaneme neprekonatelny polygon, mozeme vnutorne vrcholy odstranit z grafu
- vrchol bude bud zafixovany alebo nie, ak nebude zafixovany, bude v najmenej energeticky narocnej polohe...
- graf bude mat na zaciatku styri zafixovane vrcholy a to v rohoch siete...popriade po stranach tiez

## navrh logiky path finding session

- vyberie sa bud template TE alebo algoritmus AL alebo mapa MA
- podla toho sa vytvori ponuka pre zvysne dve veci
- po vybrani druhej sa vytvori ponuka tej poslednej
- ak sa zrusi vyber nejakeho z tychto roch atributov, znova sa prepocitaju ponuky
- ked je vybrany template TE, je umoznene vybrat model MO, ak sa vyber zrusi vyber M sa zrusi tiez a znova sa caka, kym sa vyberie tempalte

- po vybere vsetkych TE AL MA sa skontroluje korektnost vyskovych dat...ci pre vybrany algoritmus, konkretne pre jemu prisluchajuce vytvaranie grafu, je potrebne vyuzitie vyskovych dat a ak ano, tak ci pre danu polohu mapy su vyskove data stiahnute, ak nebudu upozorni uzivatela a ponukne navrat do nastaveni
- algoritmus dostane template TE, mapu MA a necha si vygenerovat reprezentaciu mapy RE, tak aby sa mapa MA uz nasledne nemusela vyuzivat

- nasledne algoritmus dostane trat TR a model MO a zacne hladat cestu CE trate TR na reprezentacii RE, ktora vracia uz finalny graf GR, ktoreho ceny hran si algoritmus AL necha vypocitat od template-u TE z modelu MO
- reprezentacia RE si ak potrebuje postupne dovytvara zo seba graf, ktory vracia algoritmu, ktory na nom nasledne hlada danu cestu


- Mapy su jedny konkretne - kazda spracovava jeden konkretny format, dedenie mozne jedine ak by jeden format bol "podformatom" druheho, proste aby to davalo zmysel
- Template-ty mozu mat potomkov
- mapove reprezentacie/grafy sa budu dat konfigurovat, pre jednu mapu a template len jedna implementacia mapovej repre
- uzivatelskych modelov moze byt pre template viac druhov a bude mozne je konfigurovat
- vyhladavacie algoritmy konfigurovatelne, pre kazdu map. repre a uzivatelsky model jedna implementacia

- mapove reprezentacie a uzivatelske modely mozu mat aj potomkov i guess, ale zretel sa na to prilis brat nebude...mozno v buducnosti aj v gui bude stromova struktura pre tieto dedicne hierarchie


- asfalt -> open terrain, dashed paths, -> rough open terrain, forrest -> 


- TODO:
  - doriesit edge case-y s prekryvom vrocholov a hran a vrcholov
  - zmenit to vytvaranie retiazky pre prekazky a znak 410_4 tak, aby sa obidve bocne retiazky vytvarali subezne - tym sa zabezpeci ze kazdy vrchol v lavej retiazke bude mat suseda v pravej - pre znak 410_4 to nieje az tak treba ale preco mat dve metody z ktorych jedna robi v podstate menej prace + lomena ciara objektu s bude spracovavat len raz, tak to zlepsi aj vykon troska :)

  USGS token IQxxyxYxTJSjzs1iOzKOg8Js7foNcu2JYo_CcTDxTDA1z@ukq_cX!FKMmsosIWjA