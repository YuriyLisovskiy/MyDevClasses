#include"./SaveDataStructures/Vector.h"

using namespace std;


int main()
{
	Vector<int> newVec;
	for(int i = 0; i < 100; i++)
	{
		newVec.pushVector(i);
	}
	for(int j = 0; j < newVec.sizeOfVector(); j++)
	{
		cout << "Element #" << j + 1 << newVec[j] << endl;
	}
	return 0;
}
