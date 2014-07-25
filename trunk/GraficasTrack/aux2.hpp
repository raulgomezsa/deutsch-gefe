#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <sstream>
std::vector<std::string> &split(const std::string &s, char delim, std::vector<std::string> &elems) {
    std::stringstream ss(s);
    std::string item;
    while (std::getline(ss, item, delim)) {
        elems.push_back(item);
    }
    return elems;
}
std::vector<std::string> split(const std::string &s, char delim) {
    std::vector<std::string> elems;
    split(s, delim, elems);
    return elems;
}
template<typename T>
std::string to_string(const T& num)
{
	std::ostringstream convert;
	convert << num;
	return convert.str();
}
double truncar(const double num){
     return ((floor(num * 100) ) / 100);
}
