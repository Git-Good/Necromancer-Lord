﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Movement : MonoBehaviour {

	public HexGrid hexGrid;
	public List<int> availablepositions;
	public List<int> possminmovepoints;

//	public List<int> GetCellIndexesOneHexAway (int index) {
//		int width = hexGrid.width;
//		int height = hexGrid.height;
//
//		List<int> positions = new List<int> ();
//		//right case
//		if ((index % width) == (width - 1)) {
//			positions.Add (index - 1);
//		}
//		//left case
//		else if ((index % width) == 0) {
//			positions.Add (index + 1);
//		} else {
//			positions.Add (index - 1);
//			positions.Add (index + 1);
//		}
//		//determine borders by even or odd rows
//		int row = index / width;
//		//Odd rows
//		if (row % 2 == 1) {
//			if ((index + width) >= (height * width - 1)) {
//				//upper is empty, only add lower cells
//				if ((index % width) != (width - 1)) {
//					//left side
//					positions.Add (index - width); //lower left
//					positions.Add (index - width + 1); //lower right
//				} else {
//					//right side
//					positions.Add (index - width); //lower left
//				}
//			} else {
//				if ((index % width) != (width - 1)) {
//					//left side
//					positions.Add (index + width); //upper left
//					positions.Add (index + width + 1); //upper right
//					positions.Add (index - width); //lower left
//					positions.Add (index - width + 1); //lower right
//				} else {
//					//right side
//					positions.Add (index + width); //upper left
//					positions.Add (index - width); //lower left
//				}
//			}
//
//		}		
//		if (row % 2 == 0) {
//
//			if ((index + width) >= (height * width - 1)) {
//				//upper is empty, only add lower cells
//				if ((index % width) != (width - 1)) {
//					//left side
//					positions.Add (index - width); //lower right
//				} else {
//					positions.Add (index - width - 1); //lower left
//					positions.Add (index - width); //lower right
//				}
//			} else if (row == 0) {
//				//lower is empty, only add upper cells
//				if ((index % width) != (width - 1)) {
//					//left side
//					positions.Add (index + width); //upper right
//				} else {
//					positions.Add (index + width - 1); //upper left
//					positions.Add (index + width); //upper right
//				}
//			} else {
//				if ((index % width) == 0) {
//					//left side
//					positions.Add (index + width); //upper right
//					positions.Add (index - width); //lower right
//				} else {
//					positions.Add (index + width - 1); //upper left
//					positions.Add (index + width); //upper right
//					positions.Add (index - width - 1); //lower left
//					positions.Add (index - width); //lower right
//				}
//			}
//		}
//		return positions;
//	}


	public List<int> GetCellIndexesOneHexAway (int index) {

		List<int> positions = new List<int> ();
		HexCoordinates coord = hexGrid.GetCellCoord (index);
		int coordx = coord.X;
		int coordz = coord.Z;

		//left and right 
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz));

		//upper left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 1));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz + 1));

		//lower left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz - 1));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 1));

		return positions;
	}

	public List<int> GetCellIndexesTwoHexAway (int index) {

		List<int> positions = new List<int> ();
		HexCoordinates coord = hexGrid.GetCellCoord (index);
		int coordx = coord.X;
		int coordz = coord.Z;

		//left and right 
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz));
		//far left and right 
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 2, coordz));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 2, coordz));

		//one level upper left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 1));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz + 1));
		//one level upper far left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 2, coordz + 1));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz + 1));

		//two level upper center
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 2));
		//two level upper left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 2, coordz + 2));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz + 2));

		//one level lower left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz - 1));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 1));
		//one level lower far left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx - 1, coordz - 1));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 2, coordz - 1));

		//two level lower center
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 2));
		//two level lower left and right
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx, coordz - 2));
		positions.Add(hexGrid.GetCellIndexFromCoord (coordx + 2, coordz - 2));

		return positions;
	}

	public List<int> GetCellIndexesBlockers (int index, int movementpoints) {
		availablepositions.Clear();

		GetCellIndexesBlockersHelper (index, movementpoints);

		return availablepositions;
	}

	public void GetCellIndexesBlockersHelper (int index, int movementpoints) {
		if (movementpoints > 0) {
			HexCoordinates coord = hexGrid.GetCellCoord (index);
			int coordx = coord.X;
			int coordz = coord.Z;

			int left = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz);
			//Debug.Log ("left"+left);
			int right = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz);
			//Debug.Log ("right"+right);
			int uleft = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 1);
			//Debug.Log ("uleft"+uleft);
			int uright = hexGrid.GetCellIndexFromCoord (coordx, coordz + 1);
			//Debug.Log ("uright"+uright);
			int lleft = hexGrid.GetCellIndexFromCoord (coordx, coordz - 1);
			//Debug.Log ("lleft"+lleft);
			int lright = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 1);
			//Debug.Log ("lright"+lright);

			//left and right 
			if (left >= 0) {
				if (hexGrid.GetEntity (left) == "Empty") {
					if (hexGrid.GetTerrain (left) == "Mountain") {
						int newmovementpoints = movementpoints - 2;
						availablepositions.Add (left);
						GetCellIndexesBlockersHelper (left, newmovementpoints);
					} else {
						int newmovementpoints = movementpoints - 1;
						availablepositions.Add (left);
						GetCellIndexesBlockersHelper (left, newmovementpoints);
					}
				}
			}
			if (right >= 0) {
				if (hexGrid.GetEntity (right) == "Empty") {
					if (hexGrid.GetTerrain (right) == "Mountain") {
						int newmovementpoints = movementpoints - 2;
						availablepositions.Add (right);
						GetCellIndexesBlockersHelper (right, newmovementpoints);
					} else {
						int newmovementpoints = movementpoints - 1;
						availablepositions.Add (right);
						GetCellIndexesBlockersHelper (right, newmovementpoints);
					}
				}
			}

			//upper left and right
			if (uleft >= 0) {
				if (hexGrid.GetEntity (uleft) == "Empty") {
					if (hexGrid.GetTerrain (uleft) == "Mountain") {
						int newmovementpoints = movementpoints - 2;
						availablepositions.Add (uleft);
						GetCellIndexesBlockersHelper (uleft, newmovementpoints);
					} else {
						int newmovementpoints = movementpoints - 1;
						availablepositions.Add (uleft);
						GetCellIndexesBlockersHelper (uleft, newmovementpoints);
					}
				}
			}
			if (uright >= 0) {
				if (hexGrid.GetEntity (uright) == "Empty") {
					if (hexGrid.GetTerrain (uright) == "Mountain") {
						int newmovementpoints = movementpoints - 2;
						availablepositions.Add (uright);
						GetCellIndexesBlockersHelper (uright, newmovementpoints);
					} else {
						int newmovementpoints = movementpoints - 1;
						availablepositions.Add (uright);
						GetCellIndexesBlockersHelper (uright, newmovementpoints);
					}
				}
			}

			//lower left and right
			if (lleft >= 0) {
				if (hexGrid.GetEntity (lleft) == "Empty") {
					if (hexGrid.GetTerrain (lleft) == "Mountain") {
						int newmovementpoints = movementpoints - 2;
						availablepositions.Add (lleft);
						GetCellIndexesBlockersHelper (lleft, newmovementpoints);
					} else {
						int newmovementpoints = movementpoints - 1;
						availablepositions.Add (lleft);
						GetCellIndexesBlockersHelper (lleft, newmovementpoints);
					}
				}
			}
			if (lright >= 0) {
				if (hexGrid.GetEntity (lright) == "Empty") {
					if (hexGrid.GetTerrain (lright) == "Mountain") {
						int newmovementpoints = movementpoints - 2;
						availablepositions.Add (lright);
						GetCellIndexesBlockersHelper (lright, newmovementpoints);
					} else {
						int newmovementpoints = movementpoints - 1;
						availablepositions.Add (lright);
						GetCellIndexesBlockersHelper (lright, newmovementpoints);
					}
				}
			}
		}
	}

	public int GetDistance (int selectedindex, int currindex) {
		HexCoordinates currcoord = hexGrid.GetCellCoord (currindex);
		int currcoordx = currcoord.X;
		int currcoordy = currcoord.Y;
		int currcoordz = currcoord.Z;
		HexCoordinates selcoord = hexGrid.GetCellCoord (selectedindex);
		int selcoordx = selcoord.X;
		int selcoordy = selcoord.Y;
		int selcoordz = selcoord.Z;
		int distance = 0;

		if (currcoordz < selcoordz) {
			if (currcoordx > selcoordx) {
				if (currcoordy <= selcoordy) {
					distance = currcoordx - selcoordx;
				} else if (currcoordy > selcoordy) {
					distance = (currcoordx - selcoordx) + (currcoordy - selcoordy);
				}
				return distance;
			} else if (currcoordx <= selcoordx) {
				distance = currcoordy - selcoordy;
				return distance;
			}
		} else if (currcoordz > selcoordz) {
			if (currcoordx <= selcoordx) {
				if (currcoordy >= selcoordy) {
					distance = selcoordx - currcoordx;
				} else if (currcoordy < selcoordy) {
					distance = (selcoordx - currcoordx) + (currcoordy - selcoordy);
				}
				return distance;
			} else if (currcoordx > selcoordx) {
				distance = selcoordy - currcoordy;
				return distance;
			}
		} else if (currcoordz == selcoordz) {
			if (currcoordx < selcoordx) {
				distance = selcoordx - currcoordx;
				return distance;
			} else if (currcoordx > selcoordx) {
				distance = currcoordx - selcoordx;
				return distance;
			} else if (currcoordx == selcoordx) {
				distance = 0;
				return distance;
			}
		}

		return distance;
	}

	public int GetMovementPointsUsed (int selindex, int currindex, int movementpoints) {
		possminmovepoints.Clear();

		GetMovementPointsUsedHelper (selindex, currindex, movementpoints, 0);

		int minmovepoints = possminmovepoints.Min ();

		return minmovepoints;

	}

	public void GetMovementPointsUsedHelper (int selindex, int currindex, int movementpoints, int usedmovementpoints) {
		if (movementpoints > 0) {
			HexCoordinates coord = hexGrid.GetCellCoord (selindex);
			int coordx = coord.X;
			int coordz = coord.Z;

			int left = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz);
			if (left == currindex) {
				possminmovepoints.Add (usedmovementpoints + 1);
			}

			int right = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz);
			if (right == currindex) {
				possminmovepoints.Add (usedmovementpoints + 1);
			}

			int uleft = hexGrid.GetCellIndexFromCoord (coordx - 1, coordz + 1);
			if (uleft == currindex) {
				possminmovepoints.Add (usedmovementpoints + 1);
			}

			int uright = hexGrid.GetCellIndexFromCoord (coordx, coordz + 1);
			if (uright == currindex) {
				possminmovepoints.Add (usedmovementpoints + 1);
			}

			int lleft = hexGrid.GetCellIndexFromCoord (coordx, coordz - 1);
			if (lleft == currindex) {
				possminmovepoints.Add (usedmovementpoints + 1);
			}

			int lright = hexGrid.GetCellIndexFromCoord (coordx + 1, coordz - 1);
			if (lright == currindex) {
				possminmovepoints.Add (usedmovementpoints + 1);
			}

			//left and right 
			if (left >= 0) {
				if (hexGrid.GetEntity (left) == "Empty") {
					if (hexGrid.GetTerrain (left) == "Mountain") {
						int newmovementpoints = movementpoints - 2;
						int newusedmovementpoints = usedmovementpoints + 2;
						GetMovementPointsUsedHelper (left, currindex, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = movementpoints - 1;
						int newusedmovementpoints = usedmovementpoints + 1;
						GetMovementPointsUsedHelper (left, currindex, newmovementpoints, newusedmovementpoints);
					}
				}
			}
			if (right >= 0) {
				if (hexGrid.GetEntity (right) == "Empty") {
					if (hexGrid.GetTerrain (right) == "Mountain") {
						int newmovementpoints = movementpoints - 2;
						int newusedmovementpoints = usedmovementpoints + 2;
						GetMovementPointsUsedHelper (right, currindex, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = movementpoints - 1;
						int newusedmovementpoints = usedmovementpoints + 1;
						GetMovementPointsUsedHelper (right, currindex, newmovementpoints, newusedmovementpoints);
					}
				}
			}

			//upper left and right
			if (uleft >= 0) {
				if (hexGrid.GetEntity (uleft) == "Empty") {
					if (hexGrid.GetTerrain (uleft) == "Mountain") {
						int newmovementpoints = movementpoints - 2;
						int newusedmovementpoints = usedmovementpoints + 2;
						GetMovementPointsUsedHelper (uleft, currindex, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = movementpoints - 1;
						int newusedmovementpoints = usedmovementpoints + 1;
						GetMovementPointsUsedHelper (uleft, currindex, newmovementpoints, newusedmovementpoints);
					}
				}
			}
			if (uright >= 0) {
				if (hexGrid.GetEntity (uright) == "Empty") {
					if (hexGrid.GetTerrain (uright) == "Mountain") {
						int newmovementpoints = movementpoints - 2;
						int newusedmovementpoints = usedmovementpoints + 2;
						GetMovementPointsUsedHelper (uright, currindex, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = movementpoints - 1;
						int newusedmovementpoints = usedmovementpoints + 1;
						GetMovementPointsUsedHelper (uright, currindex, newmovementpoints, newusedmovementpoints);
					}
				}
			}

			//lower left and right
			if (lleft >= 0) {
				if (hexGrid.GetEntity (lleft) == "Empty") {
					if (hexGrid.GetTerrain (lleft) == "Mountain") {
						int newmovementpoints = movementpoints - 2;
						int newusedmovementpoints = usedmovementpoints + 2;
						GetMovementPointsUsedHelper (lleft, currindex, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = movementpoints - 1;
						int newusedmovementpoints = usedmovementpoints + 1;
						GetMovementPointsUsedHelper (lleft, currindex, newmovementpoints, newusedmovementpoints);
					}
				}
			}
			if (lright >= 0) {
				if (hexGrid.GetEntity (lright) == "Empty") {
					if (hexGrid.GetTerrain (lright) == "Mountain") {
						int newmovementpoints = movementpoints - 2;
						int newusedmovementpoints = usedmovementpoints + 2;
						GetMovementPointsUsedHelper (lright, currindex, newmovementpoints, newusedmovementpoints);
					} else {
						int newmovementpoints = movementpoints - 1;
						int newusedmovementpoints = usedmovementpoints + 1;
						GetMovementPointsUsedHelper (lright, currindex, newmovementpoints, newusedmovementpoints);
					}
				}
			}
		}
	}
}