
Todo
====


To be fixed
-----------

* Gerer correctement les exceptions lorsque le fichier de target est invalide.

* Gerer correctement les exceptions lorsque aucun argument n'est passé en ligne de commande.

* Afficher plus de log lorsque que l'analyser d'argument plante lorsqu'un fichier n'existe pas par exemple.

* Ajouter des tests.


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

* Etudier les logs structurés avec [Serilog](https://serilog.net/)

* Possiblité de gérer des batchs de requete SQL (comme avec GO dans SSMS).
	Soit avec `GO`, soit en passant plusieurs fichier SQL en entrée. L'option avec `GO` est mieux pour les interfaces graphiques.
	`GO` est supporté par SQLCMD, OSQL et SSMS.
	La syntaxe est : `GO [count]`
	`GO` doit être sur une ligne tout seul, sans aucune commande SQL, mais la ligne peut contenir des commentaires SQL.


To be implemented (automated monitoring)
----------------------------------------

* Supprimer sonarcloud : c'est lent, ca marche mal, et c'est pas interressant sauf pour les analyses mieux faites par d'autre outils. En gros, c'est de la merde.

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

* Problème *mystérieux*. Lorsque executé en ligne de commande (dans powershell ou cmd.exe), ou dans visual studio sans l'option "Enable the Visual Studio hosting process",
il est impossible de se connecter à un serveur SQL sur un autre domaine en utilisant les informations stoquées dans le "Credential Manager" avec l'outil "cmdkey.exe".
Pourtant cela fonctionne bien dans Visual Studio lorsque l'option "Enable the Visual Studio hosting process" est activée. De même, cela fonctione aussi dans ssms.
Impossible de comprendre pourquoi, ni de trouver comment résoudre le problème. Il faut donc garder ce point pour documenter le problème, ou bien documenter la solution.
La command "runas" n'a pas aidé non plus.

* **Ne pas ajouter** un DataMerger intelligent pour offrir une alternative plus flexible au DataMerger stricte actuel.
Pas sûr sur cela soit une bonne idée étant donnée la complexité que cela ajouterait.
Surtout que cela permetterait juste d'agréger les tables avec des ensembles de colonnes différents (SELECT * ou requête dynamique).
La solution serait forcément basé sur des heuristiques. Ca laisse entrevoir beaucoup de travail et beaucoup de complexisté pour un résultat moyen.
Ca ne parait pas indispensable d'autant plus que SELECT * et les requêtes dynamiques peuvent être utilisées si une seule base de données est intérrogée.
