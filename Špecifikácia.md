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

- GUI nástroju bude spravené pomocou knižnice *win forms*
- **TODO: vyzor**

## Technická špecifikácia

nástroj bude zložený z troch hlavných častí:

1. časť slúžiaca na transformáciu mapového súboru na graf
2. algoritmická časť hľadania najkratšej cesty vo vygenerovanom grafe
3. grafická časť pre komunikáciu s uživateľom a následné grafické znázornenie výstupu algoritmickej časti

Vybraným programovacím jazykom, v ktorom bude nástroj napísaný, bude jazyk C#. Tento jazyk bol vybraný z dôvodov:

- Nástroj bude vytvárať reprezentáciu mapy a následne na nej hľadať optimálnu cestu. Takáto funkcionalita vyžaduje vyšiu výpočetnú rýchlosť a preto je lepšou voľbou ako napríklad jazyk typu python.
- Pre efektívnejšie výpočty by sa taktiež dal použiť jazyk c++ alebo jazyk c, ale v týchto jazykoch nemám dostatočné skúsenosti a efektivita jazyku c# nieje o mnoho horšia ako efektivita spomenutých nízko-úrovňových jazykov
- Taktiež nakoľko sa nechystám využívať metódy hlbokého strojového učenia, nebudú potrebné python-ovské vysokovýkonné knižnice strojového učenia
  - (prípadná možnosť využitia takýchto metód následne bude volaním externého kódu) ???
- Zároveň c# obsahuje vstavanú knižnicu *win-forms* pre vytváranie uživatelského rozhrania s ktorou som dobre oboznámený
- Ďalším možným jazykom je jazyk Java ale ako som zmienil vyššie, rád by som využil knižnicu *win-forms* a taktiež niesom s jazykom Java dostatočne oboznámený
- V neposlednom rade je programovanie v jazyku c# veľmi pohodlné a bezpečné

### 1. extrakcia grafu

- pre reporezentáciu mapy využiť elastické grafy

### 2. hľadanie najkratšej cesty

### 3. uživatelské rozhranie

- bude sa skaldať z viacerých okien - výber mapy, relevance feddback okno, okno pre vykreslenie výstupu

