using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//不適切な表現を規制するクラス
public class ForbiddenWords
{
    #region Static Fields

    //禁止ワード一覧　あまり開かないことをおすすめします
    static readonly string[] Words = new string[]{
"青姦","あおかん",
"アメ公","あめこう",
"アルコール依存症",
"犬殺し","いぬごろし",
"淫売","いんばい","売春",
"うんこ","うんこ",
"うんち",
"穢多","えた",
"ガキ","がき",
"皮被り","かわかぶり","包茎",
"姦通","かんつう",
"キ印","きじるし","精神障害者",
"キチ","きち",
"気違い",
"屑屋","くず","クズ",
"くわえ込む","くわえこむ",
"クンニ","くんに",
"強姦","ごうかん",
"ゴミ","ごみ",
"千摺り","せんずり","オナニー",
"ちんこ","チンコ","ちんぽ","チンポ","ちんちん","チンチン","ソープ",
"非人","ひにん",
"ブス","ぶす",
"部落","ぶらく",
"マンコ","まんこ","女性器","ほーみ","べちょこ","おめこ",
"セックス","せっくす"
,"あなる","アナル",
"おっぱい","オッパイ",
"死ね",
"ガイジ","がいじ",
"エロ","インポ","いんぽ","陰毛","淫乱","えっち","エッチ","おしり","顔射","がんしゃ","射精","スケベ","スカトロ","絶倫","前立腺","ちぇりーぼーい","チェリーボーイ","童貞","どうてい","てまん","手マン","なかだし","膣","発情","フェラ","ホモ","ほも","マリファナ","レイプ","性交",
"シコシコ",
"fuck","Fuck",
"shit","Shit",
"Suck","suck",
"faggot","Faggot",
"Nigger","Negro","Nigga","nigger","negro","nigga",
"sex","Sex","SEX","piss","dick","Dick","cunt","Cunt","tits","Tits",
"ass","Ass","prick","1919","4545","0721"

    };

 
    /// <summary>
    /// 禁止ワードが含まれているか判定　含まれていたらtrue　ない場合はfalse
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool WordFilter(string text)
    {
        foreach (string s in Words)
        {
            if (text.Contains(s))
            {
                return true;
            }
        }

        return false;
    }
    #endregion
}



