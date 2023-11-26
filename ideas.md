# Myslienky

- riadit sa proste standardnymi hodnotami z ISOM-u
- vyuzit reinforcement learningu, na urcenie konkretnych hodnot zpomalenia behu jednotlivymi porastami
  - tie su standartne urcene v isome do intervalov a teda by bolo fajn nechat uzivatela nech si vyberie hodnotu z danych intervalov taku, aby popisovala co najlepsie jeho predstavu idealneho postupu
- nechať uživatela si vytvárať vlasnté modely pre dné formáty máp
  - v modelu môžeme nastaviť, či chceme využiť relevance feedback a v akom intervale sa ma dany parameter nastavovany relevance feedbackom nastavovat
- spravit to cele co najobecnjesie
  - teda dovolit rozsiriteolnost o ine mapove subory
  - dovolit rozsiritelnost o nove optimalizacne algoritmy
  - 

## reprezentacia mapy

- reprezentovat mapu sietovym grafom (napriklad trojuholnikovim alebo akymkolvek inym druhom) a nechat ho ako tak pruzny v tom smere, ze ked pridam nejaky mapovy objekt, tak sa mi na jeho hrany pritiahnu najblizsie body k tymto hranam
- zaroven sa mi zmenia indikatory hran v bodoch siete na take aby zodpovedali danemu pridanemu mapovemu objektu
- taketo pritiahnute vrcholy na hranu objektu zafixujeme a nenechame nimi hybat okrem pripadu, ze su prekryte inym objektom, ktory ma vyssi level kreslenia ako ten predchadzajuci
  - v tom pripade odblokujeme vrchol a nechame ho, nech sa znova moze snapnut na hranu
- objekty budeme pridavat na zaklade ich levelu kreslenia
- ak dostaneme neprekonatelny polygon, mozeme vnutorne vrcholy odstranit z grafu
- vrchol bude bud zafixovany alebo nie, ak nebude zafixovany, bude v najmenej energeticky narocnej polohe...
- graf bude mat na zaciatku styri zafixovane vrcholy a to v rohoch siete...popriade po stranach tiez

- ku krivke budeme pritahovatvzdy jeden vrchol z hrany, ktoru dana krivka prekrizi a vzdy to bude ten, ktory je blizzsie ku prekrizeniu

## specifikacia

### co nezabudnut

- zmienit, ze sa budeme drzat ISOM 2017

## parametre, ktore treba nastavit

- vzdialenost, v ktorej sa vrcholy snapnu na objekt
- pruznost grafu
- spomalovanie jednotlivych kombinacii porastov
- pomer medzi spomalenim previsenim a mapovymi prvkami

- parametre si ulozit ako samostatny model, a aj si to udrzovat ako instanciu nejakej triedy, popripade asi skor spravit potomka danje trieda konkretne pre omap....mohol by som to spravit versitile, aby sa na to dali hladat cesty v roznych mapach....s tym ze si dotycny bude musiet napisat konvertor do svg a hlavne do grafu

## Co tam teda chcem mat

- GUI
  - hlavne menu
    - prechod do nastaveni, model-creating window, map choosing window
  - nastavenia
    - nastavenie lokalizacie
    - uzivatelske rozhranie pre stahovanie srtm dat
      - vyber zdroja vyskovych dat
      - stahovanie podla statov/vyber jednoho daneho chunk-u na mape
    - bud zapamatat posledny vyber/pracu alebo:
      - vyber defaultneho modeloveho template-u
      - vyber defaultneho algoritmu (zavisi na modelovom template-e)
      - vyber defaultného uzivatelskeho modelu(zavisi na modelovom template-e)
  - model-creating window
    - vytvaranie instancie uzivatelskeho modelu
      - vyber modeloveho template-u
      - vyber uzivatelskeho modelu
      - vyber vyhladavacieho algoritmu
      - nastavenie parametrov jednotlivych atributov instancie uzivatelskeho modelu
      - kolko prikladov sa naraz ukaze pri relevance feedback-u/kolko krat spravit relevance feedback
    - ulozenie modelu
  - map choosing window
    - vyber modeloveho template-u
    - vyber mapoveho suboru(format suboru zavisli na template-u)
    - stiahnutie potrebnych vyskovych dat (s povolenim a ak sa da), nevyzadovat, ak niesu potrebne
    - vyber modelu (v zavislosti na modelovom template-u)
    - vyber algoritmu (v zavislosti na modelovom template-u)
    - moznost vratit sa do hlavneho menu
    - prechod do relevance feedback okna
    - multiThreading, moznost zapinania viacerych vyhladavani a kazdu relaciu pustit na zvlast vlakne, obmedzit nejakym sposobom pocet spustenych relacii, potazmo vlakien
  - relevance feeback window
    - pocet okien s moznostami podla udaju v modelu
    - navrat do map choosing okna
  - path-finding window
    - vykreslenie cesty na jednoduchoej mape, ktoru si nakreslim sam a vytvorenie suboru, ktory budem moct otvorti v oomapperu a ukaze sa mi tam aj ta moja cesta
- omap to vector image/graphics/wpf-canvas convertor
- omap to MyGraf convertor
- vyhladavaci algoritmus (A*)
- interface pre MyGraph
- interface pre hladanie najrychlejsej cesty v grafe
  - necha algoritmu cely graf, nech si s nim robi co chce
  - bude vyzadovat nejaky standartny vystup z algoritmu, ktory nasledne pre vykreslovaciemu modulu, aby ho vykreslil
- interaktivne vykreslovanie najdenych ciest v mape
  - vykreslovanie vo mnou vytvorenej mape v aplikacii
  - generovanie .omap suboru s vykreslenou trasou
- vytvaranie uzivatelskych rozhrani/modelov(parametrov pre graf)
  - moznost pevneho nastavenia parametrov alebo intervalov pre relevance feeback
- requestovanie vyskovych srtm dat
  - uzivatelske rozhranie pre spravu vyskovych dat
- interface pre spracovane vyskove data
  - spracovanie SRTM dat
- relevance feedback mechanizmus
- instalacny balicek
- lokalizacia

## TODO:

naucit sa elasticke grafy
spravne si rozvrhnut projekt
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