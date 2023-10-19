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

dneska som sa zobudil na to, ze som nedokazal prestat premyslat nad sposobom, akym budem naravat vyskove udaje do aplikacie a sposobom instalacie aplikacie

- premyslam nad tym, ze uzivatel si vzdy bude moct stiahnut alebo vymazat vyskove data pre danu krajinu
- nasledne oblasti, ktore mam stiahnute by sa mohli vykreslit na nejakej mapke v gui
- premyslal som, ci tuto manipulaciu s datami a ich požadovanie nebude lahsie robit v pythonu

- v ramci instalacie by som rad pouzil klasicky sposob s uzivatelskym rozhranim
  - uzivatel si bude moct vybrat kam chce aplikaciu ulozit
  - ci chce vytvorit odkaz na ploche a (odkaz v starte)
  - akym jazykom nanho ma aplikacia rozpravat
- toto by som si ale nechal az nakoniec, az bude spravene vsetko ostatne

- tiez mi napadlo, ze asi by som mohol aplikaciu lokalizovat, popripade globalizovat
- 