CC=mcs
EXEC=GraphMonkey.exe
SHARE=/usr/local/share
BIN=/usr/local/bin

all: 
	$(CC) -target:exe -out:"$(EXEC)" -resource:./pixmaps/graphmonkey.png -resource:./pixmaps/gm.png -pkg:gtk-sharp-3.0  ./Main.cs ./GraphMonkey.cs ./Screen_Form.cs ./operation.cs ./Help_Form.cs ./About_Form.cs ./Calculus.cs

clean:
	rm -rf $(EXEC)

install:
	mkdir -p $(SHARE)/graphmonkey
	cp -f $(EXEC) $(SHARE)/graphmonkey
	cp -f graphmonkey $(BIN)
