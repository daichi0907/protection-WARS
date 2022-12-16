# protection-WARS
3年次に研究室で制作を行ったゲーム作品
  
〇制作期間  
2022/09/15～2023/01/12  
（現在も制作中のため、順次更新予定です。）

〇１週間の平均作業時間  
６～８時間（授業時間〈３時間〉含む）

〇メンバー  
・プランナー　：３名  
・デザイナー　：２名  
・プログラマー：４名　（自分 + 3名）  
  
計５名  
  
(下記に担当箇所の記載あり)  

# どんなゲームか
【ルール】  
ミニマム星の姫を敵から守る"ハンドトラッキング"を利用した「アクションゲーム」です。  
敵は姫に向かて銃やミサイルで攻撃してきます。  
様々な手段を駆使して制限時間まで姫を守り切ることでゲームクリア、制限時間内に姫の体力がなくなるとゲームオーバーとなります。  
  
【プレイヤー】
プレイヤーは手で敵の攻撃から姫を守ったり敵を叩いて攻撃することでゲームを攻略します。  
姫は摘まんだり手のひらの上に乗せることができるのでうまく攻撃を避けながら攻略することも可能です。  
  
【姫】  
敵を倒すことで経験値・回復・武器がドロップされます。  
経験値を一定数与えることで姫のレベルが上がり自衛能力が高まります。  
回復を渡すことで姫の体力を回復することができます。  
姫に武器を渡すことで武器の種類に応じた様々な自衛をしてくれます。    
  

# 担当箇所・工夫した点
- **<ins>演出関係全般</ins>**（⓵、⓷の映像参照）  
　ステージがすべて洞窟であることや「ステルスアクション」というジャンルであることから、その雰囲気がゲーム最中だけではなくゲームの導入部分から出すことでプレイヤーに没入感を与えるよう工夫しました。  
 　またチーム全体で演出に関して随時相談することで、インゲーム開始時のステージ全体のポイントとなる部分を見せる処理の追加や、ステージクリア時の達成感を与えるような演出など、ゲーム内の「間」をうまく利用した演出にすることができたと感じます。  
  
- **<ins>各分身のデータ管理</ins>**  
　各種分身のデータを一つのクラスで管理することで、インスタンス化処理やクールタイム処理をまとめてデータの参照を他の人からもしやすくなるようにしました。  
 
- **<ins>各分身の行動処理とそれに伴う敵の挙動の追加</ins>**  
　Listで探知した敵もしくは煙で見えなくなっている敵のデータを格納し、倒れたり効果が切れ消えた際にListのデータを解放することで敵の挙動がスムーズになるようにしました。  
　また、ユーザーがプレイしていて直感的に操作できるよう、ケムリスライム選択時に投げる軌道を可視化する処理をプログラミングしました。  
　敵の挙動の追加では敵の処理を主に作っていたもう一人のプログラマーと相談しあいながら処理を追加していくことで、効率よく処理の追加をすることができました。  

- **<ins>サウンド関係全般</ins>**  
　 敵やプレイヤーなどあまり私自身が関与していない部分の効果音は、下手に干渉してバグが発生しないようスクリプトを分けるなどの工夫をして実装することで、音の動機ずれや既存の処理にバグが出るような不具合などを発生させずに実装することができました。  

- **<ins>その他工夫した点</ins>**  
　プランナーが一人であることもあったため、積極的にアイデア出しや仮実装をしてチーム全体で共有するといったことをプログラマー２人で行うようにしていました。その結果プランナーの方はレベルデザインに注力することができ、効率よく制作を行うことができました。  
　随時やっている内容や終わった内容をプログラマー同士だけではなくチーム全体で共有することで、半年という短い期間でブラッシュアップまで持って行けた作品に仕上がりました。  
　プログラムのことだけでなくプランナーのレベルデザインに関する相談に乗ったり、デザイナーと演出についてアイデアや意見を持ち合わせて考えあったりすることでよりゲームらしい作品にし充てることができました。  


# ソースファイル
| ソースファイル | 軽い説明 | 記述・担当部分 |
| --- | --- | --- |


※上記に記載のないスクリプトファイルは私自身記述した部分のないスクリプトファイルとなっております。  


# 使用したデバイス・ツール
・Unity 2020.3.18f1   
・VisualStudio2019  
・Meta Quest2  
