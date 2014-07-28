CFLAGS = -ggdb
DEFINES = -DDEBUGGA
INCLUDES = 
LIBS = -lstdc++
EXAMPLE = grafica.o
CC=g++

.cpp.o:
	$(CC) -c $(CFLAGS) $(DEFINES) $(INCLUDES) $<

all::	grafica

gnuplot_i.o:	gnuplot_i.hpp
auxi.o:		auxi.hpp
grafica.o:	grafica.cpp

grafica: $(EXAMPLE)
	$(CC) -o $@ $(CFLAGS) $(EXAMPLE) $(LIBS)

clean: 
	rm -f $(EXAMPLE) grafica
