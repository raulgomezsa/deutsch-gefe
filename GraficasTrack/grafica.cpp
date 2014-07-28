#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <cmath>
#include <ctime>
#include "auxi.hpp"
#include "gnuplot_i.hpp" //Gnuplot class handles POSIX-Pipe-communikation with Gnuplot

#define SLEEP_LGTH 2  // sleep time in seconds
#define NPOINTS    50 // length of array

using namespace std;

int main(int argc, char* argv[])
{
	string file_url = "CSV/track.csv";
	string file2_url = "CSV/partida.csv";
	string file3_url = "CSV/chat.csv";
	string output = "graficas/grafica_partida";
	string output2 = "graficas/radial";
	string output3 = "graficas/radial2";
	string background = "plano/PlanoBuenoGasts";
	string bgtype = "png";
	
	cout << endl << "comienzo de la ejecución ***" << endl;
	string line, line2;
	ifstream archivo(file_url.c_str());
	
	int id_partida = -1, cond1, id_;
	float pos_x1 = -1, pos_y1 = -1, pos_x2 = -1, pos_y2 = -1, pos_x3 = -1, pos_y3 = -1, pos_x4 = -1, pos_y4 = -1;
	float p_x1 = -1, p_y1 = -1, p_x2 = -1, p_y2 = -1, p_x3 = -1, p_y3 = -1, p_x4 = -1, p_y4 = -1;
	float dist1 = 0, dist2 = 0;
	float d1 = 0, d2 = 0;
	string cond2="", cam1 = "", cam2 = "";
	
	int puntos_cam1=0, puntos_cam2=0, ncli_cam1=0, ncli_cam2=0;
	
	string legend1 = "", legend2 = "";
	
	std::vector<string> chat1, chat2, chat3, chat4;
	std::vector<double> x1, y1, x2, y2, x3, y3 ,x4, y4, x5, y5, y8, y6, y9, y7, Rp, Rm, Rc, Rpm, Rcp;

	
	if(archivo.is_open())
	{
		while( getline(archivo, line) )
		{
			vector<string> x = split(line, ';');
			
			istringstream(x[0]) >> cond1;
			cond2 = x[1];
			
			if(cond1 != id_partida)
			{
				if(!(x1.empty()) && !(x3.empty()))
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
					
					Rp.push_back(puntos_cam2);
					Rm.push_back(dist2);
					Rc.push_back(ncli_cam2);
					
					legend1 = cam1 + ": " + to_string(truncar(dist1)) + "m y " + to_string(puntos_cam1) + " puntos";
					legend2 = cam2 + ": " + to_string(truncar(dist2)) + "m y " + to_string(puntos_cam2) + " puntos";

					g2.plot_xyvector(x1, y1, x2, y2, 1, legend1);
					g2.plot_xyvector(x3, y3, x4, y4, 3, legend2);
					
					g2.savepng(output + to_string(id_partida), 4096,4096);
					g2.replot();
				}
				x1.clear(); x2.clear(); x3.clear(); x4.clear();
				y1.clear(); y2.clear(); y3.clear(); y4.clear();
				dist1 = 0; dist2 = 0;
				d1 = 0; d2 = 0;
				
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
				dist1 = dist1 + sqrt(pow(pos_x2 - pos_x1, 2) + pow(pos_y2 - pos_y1, 2));
				p_x2 = pos_x2 * 13-325;
				p_y2 = pos_y2 * 13-25;
			}
			else if(cond2.compare(cam2) == 0)
			{
				istringstream(x[2]) >> pos_x4;
				istringstream(x[3]) >> pos_y4;
				dist2 = dist2 + sqrt(pow(pos_x4 - pos_x3, 2) + pow(pos_y4 - pos_y3, 2));
				p_x4 = pos_x4 * 13-325;
				p_y4 = pos_y4 * 13-25;
			}

////////////////////////////////////////////////////////////////////////////////////////////////////
			if(pos_x1 > 0 && cond2.compare(cam1) == 0)
			{
				x1.push_back(p_x1);
				y1.push_back(p_y1);
				x2.push_back(p_x2 - p_x1);
				y2.push_back(p_y2 - p_y1);
			}
			else if(pos_x3 > 0 && cond2.compare(cam2) == 0)
			{
				x3.push_back(p_x3);
				y3.push_back(p_y3);
				x4.push_back(p_x4 - p_x3);
				y4.push_back(p_y4 - p_y3);
			}
			
			pos_x1 = pos_x2;
			pos_y1 = pos_y2;
			pos_x3 = pos_x4;
			pos_y3 = pos_y4;
			
			p_x1 = p_x2;
			p_y1 = p_y2;
			p_x3 = p_x4;
			p_y3 = p_y4;
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
		
		Rp.push_back(puntos_cam2);
		Rm.push_back(dist2);
		Rc.push_back(ncli_cam2);
		
		legend1 = cam1 + ": " + to_string(truncar(dist1)) + "m y " + to_string(puntos_cam1) + " puntos";
		legend2 = cam2 + ": " + to_string(truncar(dist2)) + "m y " + to_string(puntos_cam2) + " puntos";

		g2.plot_xyvector(x1, y1, x2, y2, 1, legend1);
		g2.plot_xyvector(x3, y3, x4, y4, 3, legend2);
		
		g2.savepng(output + to_string(id_partida), 4096,4096);
		g2.replot();
		
////////////////////////////////////////////////////////////////////////////////////////////////////
		std::ostringstream cmdstr3;
		
		float size_puntos = Rp.size();
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
			
			y6.push_back(Rm[i]/max_metros);
			
			y7.push_back((Rp[i]/Rm[i])/max_pm);
			
			y8. push_back(Rp[i]/Rc[i]/10);
			y9. push_back((Rm[i]/Rc[i])/max_mc);
		}
		
		Gnuplot g4;
		
		g4.set_radial(size_puntos,size_puntos,0,max_puntos);
		
		g4.plot_xyradial(x5, y5, 1, "Puntos");
		g4.plot_xyradial(x5, y6, 2, "Metros");
		g4.plot_xyradial(x5, y7, 3, "Pt/m");
		g4.plot_xyradial(x5, y8, 4, "Pt/cli");
		g4.plot_xyradial(x5, y9, 5, "M/cli");
		
		g4.savepng(output2, 4096,4096);
		g4.replot();
		
		Gnuplot g5;
		
		g5.set_radial(size_puntos,size_puntos,0,max_puntos);
		
		g5.plot_xyradial(x5, y7, 1, "Pt/m");
		g5.plot_xyradial(x5, y8, 2, "Pt/cli");
		g5.plot_xyradial(x5, y9, 3, "M/cli");
		
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
