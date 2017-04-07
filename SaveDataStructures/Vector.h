#pragma once
#include<iostream>
#include<string>

using namespace std;


template<class dataType>
class Vector
{
private:
	int counter;
	int size = 10;
	dataType *arr = new dataType[size];
public:
	Vector();
	Vector(const Vector &copy);
	~Vector();
	void pushVector(dataType newElement);
	dataType popVector();
	void clearVector();
	bool isEmpty();
	int sizeOfVector();
	dataType operator[] (int index);
	Vector &operator= (const Vector &newArr);
};

template<class dataType>
dataType Vector<dataType>::operator[] (int index)
{
	return arr[index];
}

template<class dataType>
Vector<dataType> &Vector<dataType>::operator= (const Vector& newArr)
{
	if (this != &newArr)
	{
		if (arr != 0)
		{
			delete[]arr;
		}
		size = newArr.size;
		arr = new dataType[size];
		for (int i = 0; i < size; ++i)
		{
			arr[i] = newArr.arr[i];
		}
	}
	return *this;
}

template<class dataType>
Vector<dataType>::Vector()
{
	counter = 0;
}

template<class dataType>
Vector<dataType>::~Vector()
{
	delete[]arr;
}

template<class dataType>
void Vector<dataType>::pushVector(dataType newElem)
{
	arr[counter] = newElem;
	counter++;
	if (counter == size)
	{
		dataType *bufArr;
		bufArr = new dataType[size];
		size += 10;
		for (int i = 0; i < size - 10; i++)
		{
			bufArr[i] = arr[i];
		}
		delete[]arr;
		arr = nullptr;
		arr = new dataType[size];
		for (int i = 0; i < size - 10; i++)
		{
			arr[i] = bufArr[i];
		}
		delete[] bufArr;
		bufArr = nullptr;
	}
}

template<class dataType>
dataType Vector<dataType>::popVector()
{
	counter--;
	return arr[counter];
}

template<class dataType>
bool Vector<dataType>::isEmpty()
{
	return counter == 0;
}

template<class dataType>
Vector<dataType>::Vector(const Vector &copy)
{
	size = copy.size;
	arr = new dataType[size];
	for (int i = 0; i < size; ++i)
	{
		arr[i] = copy.arr[i];
	}
}

template<class dataType>
int Vector<dataType>::sizeOfVector()
{
	return counter;
}

template<class dataType>
void Vector<dataType>::clearVector()
{
	delete[]arr;
	size = 10;
	counter = 0;
	arr = new dataType[size];
}
