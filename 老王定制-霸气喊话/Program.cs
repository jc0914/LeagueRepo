namespace HaydariGeceler_cici_wipi_TR
{
    using System;
    using LeagueSharp;
    using LeagueSharp.Common;

    internal class Program
    {
        private static Menu haydarigeceler;
        public static bool duramk;
        public static float gameTime1;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }
        private static void Game_OnGameLoad(EventArgs args)
        {
            Hacks.DisableSay = false;
        Game.PrintChat(
                "<font color = \"#ff052b\">鑰佺帇瀹氬埗!</font>  <font color = \"#fcdfff\">|鈽嗏槄鈽嗘父鎴忊槄鈽嗘剦蹇槄鈽嗏槄|</font> ");

            haydarigeceler = new Menu("老王VIP脚本：一鍵刷屏", "ShuaPing", true);
            var press1 =haydarigeceler.AddItem(new MenuItem("GGyaz", "“20” 方向键左键").SetValue(new KeyBind(37, KeyBindType.Press)));
            var press2=haydarigeceler.AddItem(new MenuItem("WPyaz", "“台灣罵人” 方向键右键").SetValue(new KeyBind(39, KeyBindType.Press)));
            var press3 = haydarigeceler.AddItem(new MenuItem("XDyaz", "“台灣罵人2” 方向键下键").SetValue(new KeyBind(40, KeyBindType.Press)));  
            var press4 = haydarigeceler.AddItem(new MenuItem("PNSciz", "L# 小键盘0").SetValue(new KeyBind(96, KeyBindType.Press)));
            var press5 = haydarigeceler.AddItem(new MenuItem("Smiley", "微笑 小键盘1").SetValue(new KeyBind(97, KeyBindType.Press)));
            var press6 = haydarigeceler.AddItem(new MenuItem("TRBAYRAK", "图标 小键盘2").SetValue(new KeyBind(98, KeyBindType.Press)));            
            var press7 = haydarigeceler.AddItem(new MenuItem("FCKyaz", "“台灣罵人3” 方向键上键").SetValue(new KeyBind(38, KeyBindType.Press)));
            haydarigeceler.AddItem(new MenuItem("Bilgiler", "老王漢化最强脚本VIP群：348902882"));
            haydarigeceler.AddToMainMenu();


            press1.ValueChanged += delegate
            {
                if (haydarigeceler.Item("GGyaz").GetValue<KeyBind>().Active)
                    if (duramk == false)
                    {

                        Game.Say("/all 銆€202020202020銆€2020202020銆€");
                        Game.Say("/all 銆€2020銆€202020202020銆€2020銆€");
                        Game.Say("/all 銆€銆€銆€銆€2020202020銆€銆€2020銆€");
                        Game.Say("/all 銆€銆€銆€銆€2020銆€2020銆€銆€2020銆€");
                        Game.Say("/all 銆€銆€銆€202020銆€2020銆€銆€2020銆€");
                        Game.Say("/all 銆€銆€202020銆€銆€2020銆€銆€2020銆€");
                        Game.Say("/all 銆€202020銆€銆€銆€202020202020銆€");
                        Game.Say("/all 銆€202020202020銆€20202020銆€銆€");


                        
                        duramk = true;
                        gameTime1 = Game.Time + 1;
                        

                    }
                if (Game.Time > gameTime1)
                {
                    duramk = false;
                }
                
            };
            press2.ValueChanged += delegate
            {
                if (haydarigeceler.Item("WPyaz").GetValue<KeyBind>().Active)
                    if (duramk == false)
                    {

                        Game.Say("/all 鍙扮仯瑾€滄鍣ㄨ嚜鍒垛€濓紝鍏跺鏄編鍦嬭ō瑷堛€佸彴鐏ｇ敓鐢ｏ紱鑰屼笖瓒呰泊锛屽洜鐐哄彧鏈夊彴鐏ｈ嚜宸卞湪鐢ㄤ辅");
                        Game.Say("/all 鍏ㄤ笘鐣岋紝鍙湁鍏╁€嬭鍣撳埌涓嶈鐨勫皬宄讹細 鍖楁湞楫紝 鍙扮仯銆備辅");
                        Game.Say("/all 閫欑兢鐨囧ゴ璩ょó鍌叉參鑷ぇ锛岀附浠ュ鑷繁鏄ぇ鐖猴紝鑷獚鐐鸿嚜宸辩礌璩珮浜轰竴绛夊氨鐪嬩笉璧蜂汉");
                        Game.Say("/all 鍏跺鍦ㄦ垜鐪嬩締浠栧€戞墍璎傜殑绮剧鍦ㄧ墿璩潰鍓嶅氨鏄竴娉″睅");
                        Game.Say("/all 鍙扮仯鐨勭啽琛€鎴戜及瑷堟棭鍦ㄦ姉鏃ヨ嫳闆勮帿閭ｉ閬撴埌姝荤殑閭ｄ竴鍒诲氨宸茬稉鑰楃洝浜嗕辅");
                        Game.Say("/all 閫欑兢鐣欎笅渚嗙殑浜烘牴鏈氨鏄竴缇ゆ姇闄嶈垔鏃ユ湰甯濆湅鎵嶈嫙娲讳笅渚嗙殑鍙扮仯寤㈡福");
                        Game.Say("/all 缇庡笣銆佸皬楝煎瓙鐨勮蛋鐙椼€佸€簩浠ｃ€佸€笁浠ｃ€佸厭鐨囧笣銆丟HJ銆丟NC");
                        Game.Say("/all 鎴戜篃鏄寲韬稒淇★紝涓€鍊婨閫插叆浣犺Κ濯界殑鑵愮垱瀛愬锛岄枊鍟揥Q澶у姏鎶芥彃涓変笅鎻掔殑浣犲鐖嗙偢锛屽啀涓€鍊婻鎶婁綘澶у嚭浣犺Κ濯界殑瀛愬锛屼竴鍊嬫毚鎿婃墦鐖嗕簡浣犵埜鐨勭嫍闋紝");
                        
                        duramk = true;
                        gameTime1 = Game.Time + 1;

                    }
                if (Game.Time > gameTime1)
                {
                    duramk = false;
                }
            };
            press3.ValueChanged += delegate
            {
                if (haydarigeceler.Item("XDyaz").GetValue<KeyBind>().Active)
                    if (duramk == false)
                    {

                        Game.Say("/all 鐬紒闁擄紒鐖嗭紒鐐革紒瀹岋紒鎴愶紒鍠紒娈猴紒");
                        Game.Say("/all 瑾殑濂芥湁閬撶悊.浣嗛€欎甫涓嶈兘濡ㄧ浣犳濯戒辅");
                        Game.Say("/all 鍙扮仯浜虹湡鐨勬槸寰堝鑺殑锛屼粬鍊戝皪涓栫晫銆佸皪澶ч櫢骞句箮涓€鐒℃墍鐭ワ紝鍗昏獚鐐鸿嚜宸卞緢鎳傘€備辅");
                        Game.Say("/all 鏈湡鎰忚瓨鏁欒偛宸茬稉鐣板寲鎴愪簡[涓夎嚜鎰涘湅鏈僝锛嶏紞鑷ぇ锛岃嚜鎴€锛岃嚜鐙備辅");
                        Game.Say("/all 鏈変簺鍙扮仯浜虹湡鐨勬槸鍊瘒灏忔棩鏈拰鍘熶綇姘戦洔浜よ€屼締鐨勭濂囩敓鐗╋綖锝?鎭ㄤ笉寰楀彨鍊瘒骞圭埞");
                        Game.Say("/all 瀹冨€戠編鐖硅锛氬睅鏄渶鏈夌嚐椁婄殑锛岄鍣村櫞鐨勶紝鍙互鍚冪殑锛屼簬鏄簳铔欏€戦€ｉ€ｉ粸闋ū鏄紝");
                        Game.Say("/all 鐙傚悆鑷睅锛岄倓浠ョ偤鑷繁寰堝厛閫层€傚懙鍛碉紝閫欏氨鏄簳铔欑殑寰疯锛佷辅");
                        Game.Say("/all 鍛嗙仯浜烘湁鑷敱鐨勬浠讹紝鍗绘矑鏈夎嚜鐢辩殑渚嗘簮锛岀禃澶у鏁镐汉涔熶笉閬庢槸閫氶亷鐙归殬锛屼辅");

                        duramk = true;
                        gameTime1 = Game.Time + 1;

                    }
                if (Game.Time > gameTime1)
                {
                    duramk = false;
                }
            };
            press7.ValueChanged += delegate
            {
                if (haydarigeceler.Item("FCKyaz").GetValue<KeyBind>().Active)
                    if (duramk == false)
                    {

                        Game.Say("/all 鐒＄煡锛屾硾濞涙▊鍖栫殑鍙扮仯榛撮珨渚嗗閫犱笘鐣岃锛岄€欏氨鏄偤浠€楹藉彴鐏ｅ浜曡洐鐨勫師鍥犱辅");
                        Game.Say("/all 鍗充究鍊嬪垾浜哄湪澶栭儴鐭ラ亾浜嗕竴浜涚湡鐩革紝涔熷洜鐐哄扯鍏р€滄剾鍙扮仯鈥濈殑鍙板紡鏂囬潻鐨勨€滆鎸锯€濊€屼笉鏁㈣鐪熻┍鎴栬€呭彧鏁㈣閮ㄥ垎鈥滅湡瑭扁€濓紒");
                        Game.Say("/all 閫欏氨閫犳垚浠栧€戠劇娉曟尳鏁戝扯鍏х殑浜曡洐锛侀€欑ó瑗挎柟鍦嬪鐨勭祼璜栧氨涓嶈鎷夸締鐛婚啘浜嗭紝閫欎簺绲勭箶鑳藉瑙€鐪嬪緟涓湅澶櫧鑳藉緸瑗块倞鍗囪捣锛佷辅");
                        Game.Say("/all 宸存帉澶х殑鍦版柟锛屼竴鍫嗛剦姘戜篃鏁㈢ū澶氬厓绀炬渻锛岀瑧姝讳汉銆備辅");
                        Game.Say("/all 鍙扮仯浜虹劇鐭ワ紝鍦ㄤ粬鍊戠湅渚嗕笉鏄姝わ紝浠栧€戜笉椤樻壙瑾嶈嚜宸辩劇鐭ワ紝浣犺浠栧€戠劇鐭ワ紝閭ｄ粬鍊戣偗瀹氬弽鎿婏紝鐐轰粈楹藉憿锛熶辅");
                        Game.Say("/all 鍥犵偤浠栧€戜笉椤樻壙瑾嶈嚜宸辩劇鐭ワ紝閫欐槸鏈€鏍规湰鐨勶紒");
                        Game.Say("/all 浣犲€戠湅鍙扮仯鐨勯浕瑕栫瘈鐩紝涓€涓婁締灏辫鎴戝€戝彴鐏ｆ€庨航鍏堥€叉垜鍊戝湪鍓垫剰鏂归潰涓栫晫闋樺厛锛屼辅");
                        Game.Say("/all 鐒堕€欐獢绡€鐩牴鏈氨娌掓湁浠讳綍鐨勫壍鎰忥紝鑸囨姣笉鐩搁棞鐨勭瘈鐩紝閫欏氨鏄彴鐏ｄ汉鐨勫痉鎬э紒");
 
                        duramk = true;
                        gameTime1 = Game.Time + 1;

                    }
                if (Game.Time > gameTime1)
                {
                    duramk = false;
                }
            };
            press4.ValueChanged += delegate
            {
                if (haydarigeceler.Item("PNSciz").GetValue<KeyBind>().Active)
                    if (duramk == false)
                    {

                        Game.Say("/all 銆€202020202020銆€2020202020銆€");
                        Game.Say("/all 銆€2020銆€202020202020銆€2020銆€");
                        Game.Say("/all 銆€銆€銆€銆€2020202020銆€銆€2020銆€");
                        Game.Say("/all 銆€銆€銆€銆€2020銆€2020銆€銆€2020銆€");
                        Game.Say("/all 銆€銆€銆€202020銆€2020銆€銆€2020銆€");
                        Game.Say("/all 銆€銆€202020銆€銆€2020銆€銆€2020銆€");
                        Game.Say("/all 銆€202020銆€銆€銆€202020202020銆€");
                        Game.Say("/all 銆€202020202020銆€20202020銆€銆€");

                        duramk = true;
                        gameTime1 = Game.Time + 1;

                    }
                if (Game.Time > gameTime1)
                {
                    duramk = false;
                }
            };
            press5.ValueChanged += delegate
            {
                if (haydarigeceler.Item("Smiley").GetValue<KeyBind>().Active)
                    if (duramk == false)
                    {

                        Game.Say("/all 銆€202020202020銆€2020202020銆€");
                        Game.Say("/all 銆€2020銆€202020202020銆€2020銆€");
                        Game.Say("/all 銆€銆€銆€銆€2020202020銆€銆€2020銆€");
                        Game.Say("/all 銆€銆€銆€銆€2020銆€2020銆€銆€2020銆€");
                        Game.Say("/all 銆€銆€銆€202020銆€2020銆€銆€2020銆€");
                        Game.Say("/all 銆€銆€202020銆€銆€2020銆€銆€2020銆€");
                        Game.Say("/all 銆€202020銆€銆€銆€202020202020銆€");
                        Game.Say("/all 銆€202020202020銆€20202020銆€銆€");

                        duramk = true;
                        gameTime1 = Game.Time + 1;

                    }
                if (Game.Time > gameTime1)
                {
                    duramk = false;
                }
            };
            press6.ValueChanged += delegate
            {
                if (haydarigeceler.Item("TRBAYRAK").GetValue<KeyBind>().Active)
                    if (duramk == false)
                    {

                        Game.Say("/all 銆€202020202020銆€2020202020銆€");
                        Game.Say("/all 銆€2020銆€202020202020銆€2020銆€");
                        Game.Say("/all 銆€銆€銆€銆€2020202020銆€銆€2020銆€");
                        Game.Say("/all 銆€銆€銆€銆€2020銆€2020銆€銆€2020銆€");
                        Game.Say("/all 銆€銆€銆€202020銆€2020銆€銆€2020銆€");
                        Game.Say("/all 銆€銆€202020銆€銆€2020銆€銆€2020銆€");
                        Game.Say("/all 銆€202020銆€銆€銆€202020202020銆€");
                        Game.Say("/all 銆€202020202020銆€20202020銆€銆€");
                        

                        duramk = true;
                        gameTime1 = Game.Time + 1;

                    }
                if (Game.Time > gameTime1)
                {
                    duramk = false;
                }
            };

            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            Hacks.DisableSay = false;
        }
    }
}
          

            
        

        
        
            
        
            


       