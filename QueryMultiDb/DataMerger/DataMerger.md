
###### Disclaimer

This document is in French only, because I wanted it this way.
It's just a collection of raw thoughts and facts about merging tables.
If you're willing to know what's lying in here, just use Google Translate.

# Pensées sur la fusion des tableaux de données

La tâche de fusionner les tableaux de résultats est plus complexe qu'il ne le semble aux premiers abords.

Voici une liste non exhaustive des problèmes rencontrés :
* Le nombre de tableaux varie d'une exécution à l'autre
* Le nombre de colonnes varie d'une exécution à l'autre
* Le type des colonnes varie d'une exécution à l'autre
* Le nom des colonnes varie d'une exécution à l'autre
* L'ordre des colonnes varie d'une exécution à l'autre
* Le domaine de définition d'une colonne et la sémantique de ces valeurs peut aussi changer d'une exécution à l'autre sans que ce soit même détectable
* Autre ?

Ces situations existent car les bases de données interrogées n'ont pas toutes le même schéma.
Du point de vue de cet outil, il n'est en fait pas possible de savoir si toutes les bases de données devraient réellement avoir le même schéma.

## Stratégies correctes

Les stratégies correctes donnes des résultats qui sont toujours correctes.
Elles sont conservatrices et pessimistes dans leurs choix.
Elles ne prennent que des décisions qui donnent nécessairement des résultats corrects dans tous les cas.

Exemples de stratégies :
* [ ] Fusion stricte
	* Fusionne tous les tableaux résultant d'une même exécution de requête en un seul et même tableau.
	* Si une différence est détectée entres les résultats des exécutions (nombre tableau, nombre de colonnes, types de colonnes, nom de colonnes), alors des logs décrivent toutes les différences détectées, la fusion est stoppée, aucun tableau n'est généré, un log indique qu'aucun tableau n'est généré.
	* Cette fusion comporte une phase de préanalyse qui donne une image globale des différences entre tableaux d'entrée.
* [x] Fusion conservatrice
	* Fusionne tous les tableaux résultant d'une même exécution de requête en un seul et même tableau.
	* Cette fusion comporte des règles spécifiques qui permettent de gérer simplement et efficacement les différences simples qui sont détectées.
	* Ces règles donnent nécessairement un résultat correct. Par exemple :
		* Une liste de tableaux vide (dans un résultat d'exécution) peut toujours être fusionnée avec une liste de tableaux non vides
		* Un tableau vide peut toujours être fusionné avec un tableau non vide
	* Si les tableaux comportent des différences complexe qui ne peuvent être géré par ces règles, alors la fusion est abandonnées et des logs sont émis.
* [ ] Fusion avec découpe (opportuniste)
	* Fusionne tous les tableaux qui ont le même schéma de colonnes (nombre, nom, ordre et type)
	* Si 2 tableaux issue d’une même requête ont deux schémas diffèrent, alors 2 tableaux sont générés en sortie
		* Cela implique qu'on peut avoir plusieurs onglets pour un seul résultat de requêtes pour résoudre les conflits de fusion.
* [x] Fusion nulle
	* Aucune fusion n'est réalisée.
	* Il y a autant de tableaux en entrée qu'en sortie.
		* Attention, le fichier généré peut comporter beaucoup de tableaux.
	* Cette fusion n'échoue jamais.

## Stratégies incorrectes

Les stratégies incorrectes données des résultats qui sont souvent correcte, mais peuvent parfois être faux.
Elles sont optimistes et peuvent s'appuyer sur des heuristiques qui entraine dans certains cas de mauvaises décisions.

* [ ] Fusion heuristique
	* Existence hypothétique
	* Essaye de deviner les tableaux à fusionner et la manière de les fusionner en analysant la structures des tableaux
		* Fait le pari que des tableaux suffisamment proches en structure sont probablement à fusionner ensemble
	* Problèmes :
		* Complexité importante
		* Qualité du résultat imprévisible
		* Comportement de la fusion imprévisible pour l'utilisateur
		* Une part des fusions est toujours incorrecte

## Cas de `SELECT *` et des requêtes SQL dynamiques

* Lorsqu'une seule base de données est interrogée, la fusion n'entre pas en jeu, il n'y a donc pas de problèmes
* Les stratégies correctes de _**fusion avec découpe**_ et de _**fusion nulle**_ peuvent gérer correctement ces deux points

## Notes

* Dans tous les cas la procédure de fusion joue toujours le rôle supplémentaire d'ajouter à chaque ligne de données les données des colonnes intégrées automatiquement.
	* A moins que cette partie soit faite différemment

* Réfléchir à la gestion des requetes differentes dans chaque execution.
	* Si un jeu de résultat ne pas être fusionné à cause du schema, les autre le peuvent peut être quand même

* Lorsqu'on laisse tomber la fusion de résultat, on a parfois un log WARN, alors que ERROR serait plus approrié.
