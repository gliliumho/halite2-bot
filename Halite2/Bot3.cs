using Halite2.hlt;
using System.Collections.Generic;

namespace Halite2
{
    public class MyBot
    {

        public static void Main(string[] args)
        {
            string name = args.Length > 0 ? args[0] : "Sharpie";

            Networking networking = new Networking();
            GameMap gameMap = networking.Initialize(name);

            List<Move> moveList = new List<Move>();
            for (; ; )
            {
                moveList.Clear();
                gameMap.UpdateMap(Networking.ReadLineIntoMetadata());

                foreach (Ship ship in gameMap.GetMyPlayer().GetShips().Values)
                {
                    if (ship.GetDockingStatus() != Ship.DockingStatus.Undocked)
                    {
                        continue;
                    }

                    var sorted = new SortedDictionary<double, Entity>(gameMap.NearbyEntitiesByDistance(ship));
                    foreach (KeyValuePair<double, Entity> item in sorted)
                    {
                        double distance = item.Key;
                        if (item.Key > 300)
                        {
                            continue;
                        }


                        if (item.Value.GetType() == typeof(Planet))
                        {
                            Planet planet = (Planet)item.Value;
                            if ( planet.IsOwned() && planet.GetOwner() != gameMap.GetMyPlayerId() )
                            {
                                ThrustMove newThrustMove = Navigation.NavigateShipTowardsTarget(gameMap, ship, planet, Constants.MAX_SPEED, true, 7, 0);
                                if (newThrustMove != null)
                                {
                                    moveList.Add(newThrustMove);
                                }
                                break;
                            }
                            else if ( planet.IsOwned() && planet.IsFull() )
                            {
                                continue;
                            }
                            else
                            {
                                if (ship.CanDock(planet))
                                {
                                    moveList.Add(new DockMove(ship, planet));
                                    break;
                                }

                                ThrustMove newThrustMove = Navigation.NavigateShipToDock(gameMap, ship, planet, Constants.MAX_SPEED);
                                if (newThrustMove != null)
                                {
                                    moveList.Add(newThrustMove);
                                }
                                break;
                            }

                        }

                        if (item.Value.GetType()==typeof(Ship))
                        {
                            Ship targetShip = (Ship)item.Value;
                            if ( targetShip.GetOwner() == gameMap.GetMyPlayerId() )
                            {
                                continue;
                            }
                            else
                            {
                                ThrustMove newThrustMove = Navigation.NavigateShipTowardsTarget(gameMap, ship, targetShip, Constants.MAX_SPEED, true, 7, 0);
                                if (newThrustMove != null)
                                {
                                    moveList.Add(newThrustMove);
                                }
                                break;

                            }
                        }


                    }
                    // foreach (Planet planet in gameMap.GetAllPlanets().Values)
                    // {
                    //     if (planet.IsOwned())
                    //     {
                    //         continue;
                    //     }
                    //
                    //     if (ship.CanDock(planet))
                    //     {
                    //         moveList.Add(new DockMove(ship, planet));
                    //         break;
                    //     }
                    //
                    //     ThrustMove newThrustMove = Navigation.NavigateShipToDock(gameMap, ship, planet, Constants.MAX_SPEED / 2);
                    //     if (newThrustMove != null)
                    //     {
                    //         moveList.Add(newThrustMove);
                    //     }
                    //
                    //     break;
                    // }
                }
                Networking.SendMoves(moveList);
            }
        }
    }
}
