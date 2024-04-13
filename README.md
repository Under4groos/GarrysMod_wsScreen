# wsScreen
#E2 Code 
-----------
@name FileRead
@persist File_:string 
@outputs Table:array X:number Y:number
interval(252) #80
if(first()){
    File_ = "test.txt"
    holoCreate(1)
    holoCreate(2)
    holoColor(2,vec(255,0,0))
}
else
{
    fileLoad(File_)
    String=fileRead()   
    
    Table= String:explode(":") 
    
    X = Table[1,string]:toNumber()
    Y = Table[2,string]:toNumber()
    
    
    holoPos(2, entity():toWorld(vec(X,Y,0) ) )
}
-----------
 


