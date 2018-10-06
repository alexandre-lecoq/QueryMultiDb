
Todo
====


To be fixed
-----------

* S'il y a plus de 1048576 lignes dans un onglet, excel affiche une erreur car c'est le maximum de lignes supporté par excel.
	Il faudrait détecté s'il y a trop de ligne dans ExcelExporter.
	Si c'est le cas, il supprimer le tableau et ajouté un message d'erreur dans les logs ou tronquer le tableau et ajouter un log d'erreur dans les logs en fonction d'un parametre.

* Gerer correctement les exceptions lorsque le fichier de target est invalide.

* Gerer correctement les exceptions lorsque aucun argument n'est passé en ligne de commande.

* Afficher plus de log lorsque que l'analyser d'argument plante lorsqu'un fichier n'existe pas par exemple.

* Ajouter des tests.

* Ajouter le type d'exception au message lorsqu'il y a une erreur fatal.

* Revoir les log en console pour que cele soit moins verbeux sur la console. (mettre des log au niveaux debug, et ne pas logger debug sur la console ?)

* Ne pas afficher les ExtraValue* vide dans l'onglet parametre quand aucunes valeurs n'est passée.

* Lorsqu'il n'y a qu'une seule requete et qu'elle ne renvoie rien, on trouve dans les logs le message "Merged table '' was dropped because it was empty."
	Le nom de table ne devrait pas être ''.

* Message "Tables are not identical. In <SERVER> <DATABASE> and <SERVER> <DATABASE>. Tables at index #0 have different column set."
	Alors que les deux tables ont bien le même nombre de colonnes. Quelle est la difference ?


To be implemented (short-term)
------------------------------

* Améliorer le DataMerger afin que lorsque les jeux de données d'une requete ne puisse pas être fusionée, alors que les autres résultats le peuvent, alors il devraient l'être.

* Ajouter une option `--querySTDIN` pour lire la requete SQL sur l'entrée standard.

* Ajouter une option `--outputSTDOUT` pour ecrire sur la sortie standard le fichier excel.
	Cela supprimera les logs de la sortie standard.

* Ajouter un parametre --logfile pour logger dans un fichier annexe.

* Utiliser la validation d'arguments du package CommandLineParser.

* Ajouter une interface graphique

* Ajouter un paramètre pour valider la requête sur les bases de données en utilisant `SET PARSEONLY ON;` ou `SET NOEXEC ON;` au lieu de les éxécuter.

* Ajouter un parametre en ligne de commande pour filtrer avec une expression reguliere sur les `ServerName` des fichier de targets.

* Ajouter un parametre en ligne de commande pour filtrer avec une expression reguliere sur les `DatabaseName` des fichier de targets.

* Etudier les logs structurés avec [Serilog](https://serilog.net/)

* Possiblité de gérer des batchs de requete SQL (comme avec GO dans SSMS).
	Soit avec `GO`, soit en passant plusieurs fichier SQL en entrée. L'option avec `GO` est mieux pour les interfaces graphiques.
	`GO` est supporté par SQLCMD, OSQL et SSMS.
	La syntaxe est : `GO [count]`
	`GO` doit être sur une ligne tout seul, sans aucune commande SQL, mais la ligne peut contenir des commentaires SQL.


To be implemented (automated monitoring)
----------------------------------------

* Ajouter le support de Travis CI

* Ajouter le support coveralls (cassé)

* Ajouter le support coverity

* Trouver quelque chose pour la detection de vulnerabilité et securité (en plus de coverity)

* Trouver quelque chose pour la detection de bugs et des problemes de fiabilités

* Trouver quelque chose pour la detection de la dette technique, qualité, et maintenabilité


To be implemented (long-term)
------------------------------

* Ajuster la largeur des colonnes dans le fichier Excel.

* Logger `@@SPID`

* Ajouter un mecanisme pour pouvoir choisir le nom des onglet avec des commandes dans les messages. Par exemple : `PRINT 'RenameSheet:Sheet1=' + 'nom1';`


Other
-----

* **Ne pas ajouter** un DataMerger intelligent pour offrir une alternative plus flexible au DataMerger stricte actuel.
Pas sûr sur cela soit une bonne idée étant donnée la complexité que cela ajouterait.
Surtout que cela permetterait juste d'agréger les tables avec des ensembles de colonnes différents (SELECT * ou requête dynamique).
La solution serait forcément basé sur des heuristiques. Ca laisse entrevoir beaucoup de travail et beaucoup de complexisté pour un résultat moyen.
Ca ne parait pas indispensable d'autant plus que SELECT * et les requêtes dynamiques peuvent être utilisées si une seule base de données est intérrogée.
