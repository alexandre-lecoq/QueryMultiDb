
Todo
====


To be fixed
-----------

* Gerer correctement les exceptions lorsque le fichier de target est invalide.

* Afficher plus de log lorsque que l'analyser d'argument plante lorsqu'un fichier n'existe pas par exemple.

* Ne pas afficher les ExtraValue* vide dans l'onglet parametre quand aucunes valeurs n'est passée.

* Message "Tables are not identical. In <SERVER> <DATABASE> and <SERVER> <DATABASE>. Tables at index #0 have different column set."
	Alors que les deux tables ont bien le même nombre de colonnes. Quelle est la difference ? Noms de colonnes ? Types de colonnes ?

* Ajouter des tests.


To be implemented (short-term)
------------------------------

* Améliorer le DataMerger afin que lorsque les jeux de données d'une requete ne puisse pas être fusionée, alors que les autres résultats le peuvent, alors il devraient l'être.

* Ajouter une option `--querySTDIN` pour lire la requete SQL sur l'entrée standard.

* Ajouter une option `--outputSTDOUT` pour ecrire sur la sortie standard le fichier excel.
	Cela supprimera les logs de la sortie standard.

* Ajouter un parametre --logfile pour logger dans un fichier annexe.

* Utiliser la validation d'arguments du package CommandLineParser.

* Ajouter un paramètre pour valider la requête sur les bases de données en utilisant `SET PARSEONLY ON;` ou `SET NOEXEC ON;` au lieu de les éxécuter.

* Ajouter un parametre en ligne de commande pour filtrer avec une expression reguliere sur les `ServerName` des fichier de targets.

* Ajouter un parametre en ligne de commande pour filtrer avec une expression reguliere sur les `DatabaseName` des fichier de targets.

* Ajouter un parametre permettant de tronquer le tableau exporté dans excel s'il dépasse 1048576 lignes au lieu de le supprimer.
	Ajouter un log d'erreur si le tableau est tronqué.

* Etudier les logs structurés avec [Serilog](https://serilog.net/) et avec NLog.

* Possiblité de gérer des batchs de requete SQL (comme avec GO dans SSMS).
	Soit avec `GO`, soit en passant plusieurs fichier SQL en entrée. L'option avec `GO` est mieux pour les interfaces graphiques.
	`GO` est supporté par SQLCMD, OSQL et SSMS.
	La syntaxe est : `GO [count]`
	`GO` doit être sur une ligne tout seul, sans aucune commande SQL, mais la ligne peut contenir des commentaires SQL.


To be implemented (long-term)
------------------------------

* Ajouter une interface graphique

* Ajuster la largeur des colonnes dans le fichier Excel.

* Logger `@@SPID`

* Ajouter un mecanisme pour pouvoir choisir le nom des onglet avec des commandes dans les messages. Par exemple : `PRINT 'RenameSheet:Sheet1=' + 'nom1';`


To be implemented (automated monitoring)
----------------------------------------

* Ajouter le support de Travis CI

* Ajouter le support coveralls (cassé)

* Ajouter le support coverity

* Trouver quelque chose pour la detection de vulnerabilité et securité (en plus de coverity)

* Trouver quelque chose pour la detection de bugs et des problemes de fiabilités

* Trouver quelque chose pour la detection de la dette technique, qualité, et maintenabilité
