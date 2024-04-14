# wsScreen
Проект предназначен для демоснтарации, что такое вообще можно написать и использовать. 

# Читает файл из
```
E:\Steam\steamapps\common\GarrysMod\garrysmod\data\e2files
```

# FileRead_data - Чтение файла 
```
@name FileRead_data
@persist File_:string 
@outputs String:string  
interval(80) #80
if(first()){
    File_ = "test.txt"
}
else
{
    fileLoad(File_)
    String=fileRead()   
}
``` 
# FileRead_parser - Парсер
```
@name FileRead_parser 
@outputs [ScreenSize , PositionCenter , Position]:vector2 Buttons:table Lamp:number
@inputs StringData:string 
interval(252) #80
if(first()){
    holoCreate(1)
    holoColor(1,vec4(255,255,255,100))
    holoCreate(2)
    holoColor(2,vec4(255,0,0,100))
    
    function parser_vec2(DATA:string){
        local Data_ = DATA:explode(",") 
        return vec2(Data_[1,string]:toNumber() , Data_[2,string]:toNumber())
    }   
    function parser_buttons(DATA:string){
        local Data_ = DATA:explode(",") 
        Buttons:clear()
        foreach(Variable,Value:string=Data_){
            local REPL = Value:explode(":") 
            Buttons:pushTable(table( REPL[1,string]  , REPL[2,string]:toNumber()))
        }      
    }  
     
    function parser_button_status_id(I:number){
        return Buttons[I, table ][2,number]
    }
}
else
{
    Table= StringData:explode("\n") 
    ScreenSize = parser_vec2(Table[1,string])
    PositionCenter = parser_vec2(Table[2,string])
    Position = parser_vec2(Table[3,string])
    parser_buttons(Table[4,string])
    Lamp = parser_button_status_id(1)
#[    for(I=11,10 + Buttons:count() - 1){
        
        holoCreate(I)
        holoPos(I,entity():toWorld(vec(0,I * 15 - 50,20)))
        
        
       Lamp = Buttons[I-10, table ][2,number]
        
        holoColor(I,vec(255,255,255) * Status)
        
    }]#
    holoPos(2, entity():toWorld(vec(PositionCenter:x() , PositionCenter:y()  ,0) ) )
}
```

# Build project Avalonia + .NET 7 
```
1. Клонируем репозиторий
2. Подключаем Andriod по USB к ПК 
3. Выбираем wsScreen.Android и выполняем сборку.
4. Запускаем сервер
```
https://youtube.com/shorts/TLYNM8211yM?feature=share
