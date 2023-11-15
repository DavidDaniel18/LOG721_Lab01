# LOG721 Laboratoires

# Laboratoire #2 Vue d'ensemble

## Comment démarrer

1. Démarrer Docker Desktop.
2. Lancer la commande ``docker-compose up`` à la racine du projet.
3. (... todo add trigger used for tests...)

## Cas d'utilisations
1. Map Reduce K-moyenne une itération.
2. Map Reduce K-moyenne n itérations.

## Code

- ``Node/``
- ``Docker-Compose.yml``

### Noeuds

#### *Types de noeuds*

```cs
enum NodeType {
    Map,
    Reduce
}
```

Le type de noeud permet aux services de s'enregistrer dynamiquement aux "topics" afin d'activer des fonctionnalités.

#### *Le rôle "master"*

The master node has the responsability to aggregate the results of each (*map or reduce*) node of the 
Le rôle "master" permet d'attribuer la responsabilité d'acquérir les résultats de chacun des (*Map ou Reduce*) et ensuite fait le nécessaire afin de démarrer la prochaine étape.

##### *Map "master"*

**Responsabilités:**
- Démarrage initiale.
- Initializer la cache distribuée.
- Recevoir le résultat de tous les *Map*.
- Envoyer le travail à tous les *Map*.
- Envoyer le travail à tous les *Reduce*.

##### *Reduce "master"*

**Responsabilités:**
- Déterminer la fin de la séquence d'opérations.
- Recevoir le résultat de tous les *Reduce*.
- Lancer la prochaine itération.

# Auteurs:

||
|----|
|Jérémie Bergeron|
|David Vermette Nadeau|
|Nicolas Fournier|
|Keny Noze|