using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;



public class CustomNetworkManager : NetworkManager
{
    //public UnityEvent<Alignment> onLeave = new UnityEvent<Alignment>();
    //public UnityEvent<NetworkConnectionToClient> onJoin = new UnityEvent<NetworkConnectionToClient>();
    //public UnityEvent onHost = new UnityEvent();

    public Dictionary<NetworkConnection, int> playersIndex = new Dictionary<NetworkConnection, int>();
    public PlayerData[] playerArray = new PlayerData[4];
    int connectedPlayers = 0;

    public UnityEvent onJoin;
    public UnityEvent onLeave;

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);

        //TODO: this does not work since player's object gets deleted too fast?
        //CharacterInteraction interactor = null;
        //foreach (NetworkIdentity networkIdentity in conn.clientOwnedObjects) 
        //{
        //    CharacterInteraction temp = networkIdentity.GetComponent<CharacterInteraction>();
        //    if (temp)
        //    {
        //        interactor = temp;
        //    }
        //}
        //if (interactor)
        int index = -1;
        if (playersIndex.TryGetValue(conn, out index)) //attempt to call out the alignment of the player that has left
        {
            //onLeave.Invoke(playerArray[index].alignment);

            //clean up
            playerArray[index] = new PlayerData();
            playersIndex.Remove(conn);
        }
        connectedPlayers--;

        onLeave.Invoke();
        //player has disconnected
    }
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        //onJoin.Invoke(conn);
        //player has joined

        //TODO: assign traitor on playerManager start?
        for (int i = 0; i < playerArray.Length; i++) //find empty slot in array, set it to key and index
            if (playerArray[i].alignment == Alignment.Undefined)
            {
                bool traitorMade = false;
                for (int j = 0; j < maxConnections; j++)
                {
                    if (playerArray[j].alignment == Alignment.Traitor)
                    {
                        traitorMade = true;
                        break;
                    }
                }

                //TODO: find out why there is sometimes a 2nd traitor?
                //Declare villager if there already exists a traitor, otherwise roll from 0-4, if it lands 4 (or are the last to join), declare traitor, otherwise villager
                Alignment alignment;
                //traitorMade ? Alignment.Villager : ((Random.Range(0, (maxConnections + 1)) == 0 || i == (maxConnections - 1)) ? Alignment.Traitor : Alignment.Villager);

                if (traitorMade) //if a traitor is already assigned, always set as villager
                    alignment = Alignment.Villager;
                else if (Random.Range(0, maxConnections) == 0 || i == maxConnections - 1) // 1 in 4 chance to be assigned traitor, or if no other people are traitor and you are the 4th player, become traitor
                    alignment = Alignment.Traitor;
                else //if not assigned traitor, become villager
                    alignment = Alignment.Villager;


                int modelID = -1;
                while (modelID == -1)
                {
                    int tempID = Random.Range(0, 4); //TODO: get max number of model IDs (if the max is less than the number of players, it will crash :p)

                    for (int j = 0; j < maxConnections; j++) //keep sorting through the array to find if the random ID is already used
                    {
                        if (playerArray[j].modelIndex == tempID && playerArray[j].alignment != Alignment.Undefined)
                        {
                            tempID = -1;
                            break;
                        }
                    }

                    modelID = tempID;
                }

                PlayerData temp = new PlayerData
                {
                    alignment = alignment,
                    modelIndex = (byte)modelID,
                };
                playerArray[i] = temp;
                playersIndex.Add(conn, i);
                break;
            }
        //PlayerManager.Instance.TargetAssignRole(conn, temp);

        connectedPlayers++;

        onJoin.Invoke();
        //if (connectedPlayers == 4)
        //    ChangeScene();
    }
    public override void OnStartHost()
    {
        base.OnStartHost();
        //onHost.Invoke();
        //started hosting
        playersIndex.Clear(); //empty dictionary of player data
        playerArray = new PlayerData[4];
    }

    [Server]
    public void ChangeScene()
    {
        ServerChangeScene("SampleScene");
    }
}
