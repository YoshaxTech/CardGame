# CARDGAMES

# CONNECTION

la connection entre le serveur et le client ce fait grace à la librairie networkcoms.net.
notre communication est basé sur l'envoie d'un object Message:
```
public class Message {
    public String      Action;
    public String      Message;
}
```
Les différente Action et Message vont correspondre au différentes phases du jeu.

# SERVEUR
Le serveur va s'occuper de gérer l'algorythme du jeu et de communiqué les différentes action au différents client en fonction de la phases de jeu.
Il recevras généralement des object Card:
```
public class        Card {
    public String     _sign;
    public String     _rank;
}
```
Il reçois aussi des object Message.
- Flags Action reçu : 
  
  - card : 
  il recois la crate joué par le client
  - bonjours : 
  indique quand un client ce connecte
  
# CLIENT
Le client va recevoir des object de type Message afin de savoir quelle action affectuer.
- Flags Action reçu
  
  - Bienvenue
  indique qu la partie va commencé
  - card
  recois un object card
  - You
  indique au client que c'est a sont tour de joué 
  - End
  indique la fin de la partie.
  
