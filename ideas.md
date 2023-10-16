# Myslienky

- spravit nejaky research toho, aky je rozdiel medzi behanim v roznych porastoch
- nemetricke MDS
- pouzit nieco, co mi bude simulovat rozhodovaci proces pretekara...ze ked vidim ze to inakadial nejde, tak tomu proste trocha zvysimm preferenciu
- alebo vazne to robit nejak iterativne s tym ze vstup bude len porovnanie tych typov porastu a terenu
- riadit sa proste standardnymi hodnotami z ISOM-u

- vyuzit reinforcement learningu, na urcenie konkretnych hodnot zpomalenia behu jednotlivymi porastami
  - tie su standartne urcene v isome do intervalov a teda by bolo fajn nechat uzivatela nech si vyberie hodnotu z danych intervalov taku, aby popisovala co najlepsie jeho predstavu idealneho postupu

## reprezentacia mapy

- reprezentovat mapu sietovym grafom (napriklad trojuholnikovim alebo akymkolvek inym druhom) a nechat ho ako tak pruzny v tom smere, ze ked pridam nejaky mapovy objekt, tak sa mi na jeho hrany pritiahnu najblizsie body k tymto hranam
- zaroven sa mi zmenia indikatory hran v bodoch siete na take aby zodpovedali danemu pridanemu mapovemu objektu
- taketo pritiahnute vrcholy na hranu objektu zafixujeme a nenechame nimi hybat okrem pripadu, ze su prekryte inym objektom, ktory ma vyssi level kreslenia ako ten predchadzajuci
  - v tom pripade odblokujeme vrchol a nechame ho, nech sa znova moze snapnut na hranu
- objekty budeme pridavat na zaklade ich levelu kreslenia
- ak dostaneme neprekonatelny polygon, mozeme vnutorne vrcholy odstranit z grafu

- ku krivke budeme pritahovatvzdy jeden vrchol z hrany, ktoru dana krivka prekrizi a vzdy to bude ten, ktory je blizzsie ku prekrizeniu

## specifikacia

### co nezabudnut

- zmienit, ze sa budeme drzat ISOM 2017

## parametre, ktore treba nastavit

- vzdialenost, v ktorej sa vrcholy snapnu na objekt
- pruznost grafu



## TODO:

naucit sa elasticke grafy
spravne si rozvrhnut projekt