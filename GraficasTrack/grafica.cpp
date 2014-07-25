#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <cmath>
#include "aux2.hpp"
#include "gnuplot_i.hpp" //Gnuplot class handles POSIX-Pipe-communikation with Gnuplot

#define SLEEP_LGTH 2  // sleep time in seconds
#define NPOINTS    50 // length of array

using namespace std;

int main(int argc, char* argv[])
{
	string file_url = "CSV/track.csv";
	string file2_url = "CSV/partida.csv";
	string output = "graficas/grafica_partida";
	string output2 = "graficas/radial";
	string output3 = "graficas/radial2";
	string background = "plano/PlanoBuenoGasts";
	string bgtype = "png";
	/*
	for(int i=1; i < argc; ++i)
	{
		string opt = argv[i];
		
		switch(opt)
		{
			case "-f": ++i;
				file_url = argv[i];
				break;
			case "-bg": ++i;
				background = (split(argv[i], '.'))[0];
				bgtype = (split(argv[i], '.'))[1];
				break;
			case "-o": ++i;
				output = argv[i];
				break;
			default:
				cout << "Error" << endl;
				exit(1);
    				break;
		}
	}*/
	
	cout << endl << "comienzo de la ejecución ***" << endl;
	string line, line2;
	ifstream archivo(file_url.c_str());
	
	int id_partida = -1, cond1, id_;
	float pos_x1 = -1, pos_y1 = -1, pos_x2 = -1, pos_y2 = -1, pos_x3 = -1, pos_y3 = -1, pos_x4 = -1, pos_y4 = -1;
	float dist1 = 0, dist2 = 0;
	string cond2="", cam1 = "", cam2 = "";
	
	int puntos_cam1=0, puntos_cam2=0, ncli_cam1=0, ncli_cam2=0;
	
	string legend1 = "", legend2 = "";
	
	std::vector<double> x1, y1, x2, y2, x3, y3 ,x4, y4, x5, y5, x6, y6, x7, y7, Rp, Rm, Rc, Rpm, Rcp;
	
	if(archivo.is_open())
	{
		while( getline(archivo, line) )
		{
			vector<string> x = split(line, ';');
			
			istringstream(x[0]) >> cond1;
			cond2 = x[1];
			
			if(cond1 != id_partida)
			{
				if(!(x1.empty()) && !(x3.empty()) /*&& id_partida < 4*/)
				{
					cout << "Grafica de partida " << id_partida << endl;
					Gnuplot g2;
					
					g2.set_background(background, bgtype, 637, 637);
					
					g2.cmd("set key at 550,70");
					
					ifstream archivo2(file2_url.c_str());
					if(archivo2.is_open()){
						while( getline(archivo2, line2) && id_partida!=id_)
						{
							vector<string> y = split(line2, ';');
							
							id_=0; puntos_cam1=0; puntos_cam2=0; ncli_cam1=0; ncli_cam2=0;
							
							istringstream(y[0]) >> id_;
							istringstream(y[2]) >> puntos_cam1;
							istringstream(y[5]) >> puntos_cam2;
							istringstream(y[3]) >> ncli_cam1;
							istringstream(y[6]) >> ncli_cam2;
						}
						
					}
					archivo2.close();
					
					Rp.push_back(puntos_cam1);
					Rm.push_back(dist1);
					Rc.push_back(ncli_cam1);
					//Rpm.push_back(puntos_cam1/dist1);
					//Rcp.push_back(ncli_cam1/puntos_cam1);
					
					Rp.push_back(puntos_cam2);
					Rm.push_back(dist2);
					Rc.push_back(ncli_cam2);
					//Rpm.push_back(puntos_cam2/dist2);
					//Rcp.push_back(ncli_cam2/puntos_cam2);
					
					legend1 = cam1 + ": " + to_string(truncar(dist1)) + "m y " + to_string(puntos_cam1) + " puntos";
					legend2 = cam2 + ": " + to_string(truncar(dist2)) + "m y " + to_string(puntos_cam2) + " puntos";

					g2.plot_xyvector(x1, y1, x2, y2, 1, legend1);
					g2.plot_xyvector(x3, y3, x4, y4, 3, legend2);
					
					//g2.savetops(output + to_string(id_partida) );
					//g2.replot();
					g2.savepng(output + to_string(id_partida), 4096,4096);
					g2.replot();
				}
				x1.clear(); x2.clear(); x3.clear(); x4.clear();
				y1.clear(); y2.clear(); y3.clear(); y4.clear();
				dist1 = 0; dist2 = 0;
				
				id_partida = cond1;
				cam1 = cond2;
				cam2 = "";
			}
			else if(cond2!=cam1 && cam2=="")
			{
				cam2 = cond2;
			}
			
////////////////////////////////////////////////////////////////////////////////////////////////////
			if(cond2.compare(cam1) == 0)
			{
				istringstream(x[2]) >> pos_x2;
				istringstream(x[3]) >> pos_y2;
				pos_x2 = pos_x2 * 13-325;
				pos_y2 = pos_y2 * 13-25;
			}
			else if(cond2.compare(cam2) == 0)
			{
				istringstream(x[2]) >> pos_x4;
				istringstream(x[3]) >> pos_y4;
				pos_x4 = pos_x4 * 13-325;
				pos_y4 = pos_y4 * 13-25;
			}

////////////////////////////////////////////////////////////////////////////////////////////////////
			if(pos_x1 > 0 && cond2.compare(cam1) == 0)
			{
				x1.push_back(pos_x1);
				y1.push_back(pos_y1);
				x2.push_back(pos_x2 - pos_x1);
				y2.push_back(pos_y2 - pos_y1);
				
				dist1 = dist1 + sqrt(pow((pos_x2+325)/13 - (pos_x1+325)/13, 2) + pow((pos_y2+25)/13 - (pos_y1+25)/13, 2));
			}
			else if(pos_x3 > 0 && cond2.compare(cam2) == 0)
			{
				x3.push_back(pos_x3);
				y3.push_back(pos_y3);
				x4.push_back(pos_x4 - pos_x3);
				y4.push_back(pos_y4 - pos_y3);
				
				dist2 = dist2 + sqrt(pow((pos_x4+325)/13 - (pos_x3+325)/13, 2) + pow((pos_y4+25)/13 - (pos_y3+25)/13, 2));
			}
			
			pos_x1 = pos_x2;
			pos_y1 = pos_y2;
			pos_x3 = pos_x4;
			pos_y3 = pos_y4;	
		}
		archivo.close();
		
////////////////////////////////////////////////////////////////////////////////////////////////////
		
		cout << "Grafica de partida " << id_partida << endl;
		Gnuplot g2;
		
		g2.set_background(background, bgtype, 637, 637);
		g2.cmd("set key at 550,70");
		
		ifstream archivo2(file2_url.c_str());
		if(archivo2.is_open()){
			while( getline(archivo2, line2) && id_partida!=id_)
			{
				vector<string> y = split(line2, ';');
				
				id_=0; puntos_cam1=0; puntos_cam2=0; ncli_cam1=0; ncli_cam2=0;
				
				istringstream(y[0]) >> id_;
				istringstream(y[2]) >> puntos_cam1;
				istringstream(y[5]) >> puntos_cam2;
				istringstream(y[3]) >> ncli_cam1;
				istringstream(y[6]) >> ncli_cam2;
			}
			
		}
		archivo2.close();
		
		Rp.push_back(puntos_cam1);
		Rm.push_back(dist1);
		Rc.push_back(ncli_cam1);
		//Rpm.push_back(puntos_cam1/dist1);
		//Rcp.push_back(ncli_cam1/puntos_cam1);
		
		Rp.push_back(puntos_cam2);
		Rm.push_back(dist2);
		Rc.push_back(ncli_cam2);
		//Rpm.push_back(puntos_cam2/dist2);
		//Rcp.push_back(ncli_cam2/puntos_cam2);
		
		legend1 = cam1 + ": " + to_string(truncar(dist1)) + "m y " + to_string(puntos_cam1) + " puntos";
		legend2 = cam2 + ": " + to_string(truncar(dist2)) + "m y " + to_string(puntos_cam2) + " puntos";

		g2.plot_xyvector(x1, y1, x2, y2, 1, legend1);
		g2.plot_xyvector(x3, y3, x4, y4, 3, legend2);
		
		//g2.savetops(output + to_string(id_partida) );
		//g2.replot();
		g2.savepng(output + to_string(id_partida), 4096,4096);
		g2.replot();
		
		
		
////////////////////////////////////////////////////////////////////////////////////////////////////
		std::ostringstream cmdstr3;
		
		int size_puntos = Rp.size();
		float max_metros = 0;
		float max_pm = 0;
		float max_mc = 0;
		int max_puntos = 0;
		
		
		
		for(int i = 0; i < size_puntos; ++i)
		{			
			if(max_metros < Rm[i])
				max_metros = Rm[i];
			
			if(max_puntos < Rp[i])
				max_puntos = Rp[i];
			
			if(max_pm < Rp[i]/Rm[i])
				max_pm = Rp[i]/Rm[i];
				
			if(max_mc < Rm[i]/Rc[i])
				max_mc = Rm[i]/Rc[i];
		}
		
		for(int i = 0; i < size_puntos; ++i)
		{
			x5.push_back(360-((360/size_puntos)*(Rm.size()-i)));
			y5.push_back(Rp[i]/max_puntos);
			
			//cout << 360-((360/size_puntos)*(Rm.size()-i)) << endl;
			//cout << Rp[i]/max_puntos << endl;
			
			//x6.push_back(360-((360/size_puntos)*(Rm.size()-i)));
			y6.push_back(Rm[i]/max_metros);
			
			//x7.push_back(360-((360/size_puntos)*(Rm.size()-i)));
			y7.push_back((Rp[i]/Rm[i])/max_pm);
			
			x6. push_back(Rp[i]/Rc[i]/10);
			x7. push_back((Rm[i]/Rc[i])/max_mc);
			
			if(Rm[i]/max_metros == 1 || Rm[i]/max_metros == 0)
				cout << "Alumno " << i+1 << ", metros recorridos \t" << Rm[i]/max_metros << "m,\t puntos " << Rp[i]/max_puntos << ",\t Pt/m " << (Rp[i]/Rm[i])*10 << endl;
			else if(Rp[i]/max_puntos == 1 || Rp[i]/max_puntos == 0)
				cout << "Alumno " << i+1 << ", metros recorridos " << Rm[i]/max_metros << "m,\t puntos \t" << Rp[i]/max_puntos << ",\t Pt/m " << (Rp[i]/Rm[i])*10 << endl;
			else
				cout << "Alumno " << i+1 << ", metros recorridos " << Rm[i]/max_metros << "m,\t puntos " << Rp[i]/max_puntos << ",\t Pt/m " << (Rp[i]/Rm[i])*10 << endl;
		}
		
		Gnuplot g4;
		
		g4.set_radial(size_puntos,size_puntos,0,max_puntos);
		
		g4.plot_xyradial(x5, y5, 1, "Puntos");
		g4.plot_xyradial(x5, y6, 2, "Metros");
		g4.plot_xyradial(x5, y7, 3, "Pt/m");
		g4.plot_xyradial(x5, x6, 4, "Pt/cli");
		g4.plot_xyradial(x5, x7, 5, "M/cli");
		
		g4.savepng(output2, 4096,4096);
		g4.replot();
		
		Gnuplot g5;
		
		g5.set_radial(size_puntos,size_puntos,0,max_puntos);
		
		g5.plot_xyradial(x5, y7, 1, "Pt/m");
		g5.plot_xyradial(x5, x6, 2, "Pt/cli");
		g5.plot_xyradial(x5, x7, 3, "M/cli");
		
		g5.savepng(output3, 4096,4096);
		g5.replot();
	}	
	else
	{
		cout << "Imposible abrir el archivo" << endl;
	}
	
	cout << endl << "*** fin de la ejecución" << endl;

	return 0;
}
