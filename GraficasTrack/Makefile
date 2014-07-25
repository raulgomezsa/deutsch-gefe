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
aux2.o:	aux2.hpp
grafica.o:	grafica.cpp

grafica: $(EXAMPLE)
	$(CC) -o $@ $(CFLAGS) $(EXAMPLE) $(LIBS)

clean: 
	rm -f $(EXAMPLE) grafica
