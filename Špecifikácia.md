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

- pre reporezentáciu mapy využiť elastické grafy
- 