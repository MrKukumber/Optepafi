# Špecifikácia - O-path finder

- nástroj pre hľadanie najrýchlejsích ciest v otvorenom terénne na základe mapy využívanej v športovej disciplíne orientačný beh

## Požadované funkcionality

nástroj by mal byť schopný:

- spracovať vstupný mapový súbor formátu *OpenOrienteering Mapper (ďalej len omap)* spolu so súborom obsahujúcim výškové údaje danej mapy formátu **TODO:**
- nechať uživatela nastaviť potrebné parametry grafovej reprezentácie daného mapového súboru
- nechať uživatela interaktívne pomocou metódy *relevance feedback* nastaviť presné parametre modelu
- graficky znázorniť nájdenú najrýchlejšiu cestu medzi dvomi uživatelom zadanými bodmi na mape

## Požadované vlastnosti

- rozumný čas spracovania mapového súboru
- rozumný čas hľadania najrýchlejšej cesty vo vygenerovanom grafe
- možnoť rozširitelnosti programu o rôzne metódy hľadania najr. cesty

## Dizajnová špecifikácia

- GUI nástroju bude spravené pomocou knižnice *win forms / WPF ????*
- **TODO: vyzor**

## Technická špecifikácia

nástroj bude zložený z troch hlavných častí:

1. časť slúžiaca na transformáciu mapového súboru na graf
2. algoritmická časť hľadania najkratšej cesty vo vygenerovanom grafe
3. grafická časť pre komunikáciu s uživateľom a následné grafické znázornenie výstupu algoritmickej časti

Súčasťou by mala byť aj možnosť spravovať nastavenia nástroju (jazyk, stiahnute výškove dáta, ...) a inštalačný balíček

Vybraným programovacím jazykom, v ktorom bude nástroj napísaný, bude jazyk C#.

### Inštalácia

- TODO:

### Nastavenia

- nastavenie jazyka, sprava vyskovych dat

### 1. extrakcia grafu

- pre reporezentáciu mapy využiť elastické grafy

- pre výškové informácie využijem globálny dataset SRTM s rozlíšením 30 metrov
- uživatel si bude môcť vybrať, ktoré regióny si chce stiahnuť alebo po pridaní mapového súboru si môže nechať stiahnuť automaticky regióny, ktoré sú potrebné ku danému súboru
- sťahovanie budem riešiť pomocou práce s USGS EROS Api a dotazovania sa nanho o data

### 2. hľadanie najkratšej cesty

- využitie *relevance feedbeck* pre nastavenie parametrov modelu

### 3. uživatelské rozhranie

- bude sa skaldať z viacerých okien - výber mapy, relevance feddback okno, okno pre vykreslenie výstupu, okno nastavení, development okno
  - uživatel si bude môcť vytvoriť vlastný parametrický model a uložiť si ho
- bude potrebné vytvoriť konvertor z súboru typu omap do vektorovej grafiky
- TODO: bud pouzijem knizinicu Graphics zo c# alebo draw svg z pythonu


