#pragma once
#include<fstream>
#include<string>
#include<iostream>

using namespace std;


class FileManager
{
private:
	bool checkRead;
	bool checkWrite;
	string filePath;
	ofstream _fileWrite;
	ifstream _fileRead;
public:
	FileManager(): filePath("NewFile.txt"), checkRead(false), checkWrite(false) {}
	FileManager(string filePath): filePath(filePath + ".txt"), checkRead(false), checkWrite(false) {}
	void writeStr(string fileData);
	void writeDouble(double fileData);
	string readStr();
	void nextLine();
	bool isWritten();
	bool isRead();
};

void FileManager::writeStr(string fileData)
{
	this->_fileWrite.open(this->filePath, ios::app);
	if (this->_fileWrite.is_open())
	{
		this->_fileWrite << fileData;
		this->_fileWrite.close();
		this->checkWrite = true;
	}
	else
	{
		cout << "Open error: File '" << this->filePath << "' was not opened for writting!" << endl;
		return;
	}
}

void FileManager::writeDouble(double fileData)
{
	this->_fileWrite.open(this->filePath, ios::app);
	if (this->_fileWrite.is_open())
	{
		this->_fileWrite << fileData;
		this->_fileWrite.close();
		this->checkWrite = true;
	}
	else
	{
		cout << "Open error: File '" << this->filePath << "' was not opened for writting!" << endl;
		return;
	}
}

template<class dataType>
dataType FileManager::readStr()
{
	dataType readData;
	if (!this->_fileRead.is_open())
	{
		this->_fileRead.open(this->filePath);
	}
	if (this->_fileRead.is_open())
	{
		getline(this->_fileRead, readData);
		if (this->_fileRead.eof())
		{
			this->_fileRead.close();
		}
		this->checkRead = true;
	}
	else
	{
		cout << "Open error: File '" << this->filePath << "' was not opened for reading!" << endl;
	}
	return readData;
}

void FileManager::nextLine()
{
	this->_fileWrite.open(this->filePath, ios::app);
	if (this->_fileWrite.is_open())
	{
		this->_fileWrite << '\n';
		this->_fileWrite.close();
		this->checkWrite = true;
	}
	else
	{
		cout << "Open error: File with " << this->filePath << " directory was not opened for writting!" << endl;
		return;
	}
}

bool FileManager::isWritten()
{
	return this->checkWrite;
}

bool FileManager::isRead()
{
	return this->checkRead;
}
