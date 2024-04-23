# Myslienky

- riadit sa proste standardnymi hodnotami z ISOM-u
- vyuzit reinforcement learningu, na urcenie konkretnych hodnot zpomalenia behu jednotlivymi porastami
  - tie su standartne urcene v isome do intervalov a teda by bolo fajn nechat uzivatela nech si vyberie hodnotu z danych intervalov taku, aby popisovala co najlepsie jeho predstavu idealneho postupu
- nechať uživatela si vytvárať vlasnté modely pre dné formáty máp
- spravit to cele co najobecnjesie
  - teda dovolit rozsiritelnost o ine mapove subory a ich graficku repre
  - dovolit rozsiritelnost o nove optimalizacne algoritmy
  - o rozne sposoby vytvarania grafu z mapoveho suboru (proste uzavriet tu agregaciu toho grafu do nejakej triedy tak, aby sa to dalo jednoucho vymenit za nieco ine), asi o vsak nebudem uvadzat ako moznost na vyber v nastaveniach....len proste keby to tam chcel niekto v buducnosti prid, tak nech to neni pain
- MVVM architektúra
  - model view bude pocuvat na prikazy view a na zaklade nich bude podnecovat session controller-y k vykonavaniu postupnych cinnosti za pomoci modelu
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

## Co budem ukaldat

- vyskove data (pre kazdy zdroj zvlast)
- uzivatelske modely
- mapove formaty s pridanymi najdenymi cestami
- ukladame pre opatovne spustenie aplikacie
  - posledne pouzity algoritmus, tameplate, model, posledny zdroj vyskovych dat, mapa - parametry
  - posledne pouzity zdroj vyskovych dat, stiahnute regiony

## TODO:

naucit sa elasticke grafy
poriadne premysliet ako pritahovat tie vrcholy k tym objektom, hlavne v pripadoch, kedy mi jeden objekt krizuje hranu viac krat

## toto si tu odlozim

dovody, preco C#:

- Nástroj bude vytvárať reprezentáciu mapy a následne na nej hľadať optimálnu cestu. Takáto funkcionalita vyžaduje vyšiu výpočetnú rýchlosť a preto je lepšou voľbou ako napríklad jazyk typu python.
- Pre efektívnejšie výpočty by sa taktiež dal použiť jazyk c++ alebo jazyk c, ale v týchto jazykoch nemám dostatočné skúsenosti a efektivita jazyku c# nieje o mnoho horšia ako efektivita spomenutých nízko-úrovňových jazykov
- Taktiež nakoľko sa nechystám využívať metódy hlbokého strojového učenia, nebudú potrebné python-ovské vysokovýkonné knižnice strojového učenia
  - TODO: (prípadná možnosť využitia takýchto metód následne bude volaním externého kódu) ???
- Zároveň c# obsahuje vstavanú knižnicu *win-forms* pre vytváranie uživatelského rozhrania s ktorou som dobre oboznámený
- Ďalším možným jazykom je jazyk Java ale ako som zmienil vyššie, rád by som využil knižnicu *win-forms* a taktiež niesom s jazykom Java dostatočne oboznámený
- V neposlednom rade je programovanie v jazyku c# veľmi pohodlné a bezpečné

---

preco WPF:

- modernejsi nastroj nez winforms
- vektorova grafika
- modernejsi vzhlad

nevyhody:

- musim sa naucit, neovladam tolko ako winfomrs

---

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





- vyberie sa bud template TE alebo algoritmus AL alebo mapa MA
- podla toho sa vytvori ponuka pre zvysne dve veci
- po vybrani druhej sa vytvori ponuka tej poslednej
- ak sa zrusi vyber nejakeho z tychto roch atributov, znova sa prepocitaju ponuky
- ked je vybrany template TE, je umoznene vybrat model MO, ak sa vyber zrusi vyber M sa zrusi tiez a znova sa caka, kym sa vyberie tempalte

- po vybere vsetkych TE AL MA sa skontroluje korektnost vyskovych dat...ci pre vybrany algoritmus, konkretne pre jemu prisluchajuce vytvaranie grafu, je potrebne vyuzitie vyskovych dat a ak ano, tak ci pre danu polohu mapy su vyskove data stiahnute, ak nebudu upozorni uzivatela a ponukne navrat do nastaveni
- algoritmus dostane template TE, mapu MA a necha si vygenerovat reprezentaciu mapy RE, tak aby sa mapa MA uz nasledne nemusela vyuzivat

- nasledne algoritmus dostane trat TR a model MO a zacne hladat cestu CE trate TR na reprezentacii RE, ktora vracia uz finalny graf GR, ktoreho ceny hran si algoritmus AL necha vypocitat od template-u TE z modelu MO
- reprezentacia RE si ak potrebuje postupne dovytvara zo seba graf, ktory vracia algoritmu, ktory na nom nasledne hlada danu cestu