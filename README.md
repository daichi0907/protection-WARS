# 守護WARS
3年次の後期に研究室でチーム制作を行ったゲーム。  

 ![VR_Test_ShugoWars_1208_プレイ写真_Moment](https://user-images.githubusercontent.com/71632844/208294413-4645f3e9-b6fc-4414-bb3f-e596b33a72bb.jpg)
 
〇制作期間  
2022/09/15～2023/01/12  

〇１週間の平均作業時間  
６～８時間（授業時間〈３時間〉含む）

〇メンバー  
・プランナー　：３名  
・デザイナー　：２名  
・プログラマー：４名　（自分 + 3名）  
  
計９名  
  
(下記に担当箇所の記載あり)  


# ファイルの説明
- **<ins>VR_ShugoWarsフォルダ</ins>**  
　⇒本ゲームのプロジェクトフォルダ  
  
- **<ins>README.md</ins>**  
　⇒ゲーム概要などの説明  
  
- **<ins>プレイ時の注意事項「守護WARS」.pdf</ins>**  
　⇒ゲームをプレイしていただく際の注意事項が記述されております。  
  
- **<ins>実行ファイル</ins>**  
　⇒ギガファイル便URL：**<ins>[実行ファイルダウンロード](https://54.gigafile.nu/0117-k09c2aaccae4e2e14745fbf66015e65c7)</ins>**（期間：2023年1月17日(火)まで, ダウンロードキー：無し）   
  <!--削除キー375d-->

- **<ins>プレイ映像（動画）</ins>**  
　⇒ギガファイル便URL：**<ins>[プレイ映像ダウンロード](https://41.gigafile.nu/0120-e011ac22a6f3e1ab490e4b06b16ee259d)</ins>**（期間：2023年1月20日(金)まで, ダウンロードキー：無し）  
  <!--削除キーf59d-->
  
※「実行ファイル」と「プレイ映像」ですが、  
　gitですと容量の関係上アップすることができませんでしたので、  
　上記のURLからダウンロードをお願いいたします。  
　（製作途中のため、完成品ができ次第ファイル内容並びにURLが変更する可能性があります。）


# どんなゲームか
【ルール】  
手のひらサイズの姫を敵から護る"ハンドトラッキング"を利用した「アクションゲーム」です。  
敵は姫に向かって銃やミサイルで攻撃してきます。  
様々な手段を駆使して制限時間まで姫を守り切ることでゲームクリア、制限時間内に姫の体力がなくなるとゲームオーバーとなります。  
  
【プレイヤー】  
プレイヤーは手で敵の攻撃から姫を護ったり敵を叩いて攻撃することでゲームを攻略していきます。  
姫は摘待むことができたり、手のひらの上に乗せることができるので、うまく攻撃を避けながら攻略することも可能です。  
  
【姫】  
敵を倒すことで経験値・回復・武器がドロップされます。  
経験値を一定数与えることでレベルが上がりプレイヤーと姫の体力が満タンになります。  
回復を渡すことで姫の体力を回復することができます。  
  
![VR_Test_ShugoWars_1208_セットアップフェーズ映像](https://user-images.githubusercontent.com/71632844/208294900-5f8c6a7d-398d-411b-8788-2ab3f9af44f8.gif)  
⓵セットアップフェーズの映像  
  
![VR_Test_ShugoWars_1208_プレイ映像](https://user-images.githubusercontent.com/71632844/208294927-dde3571f-8cd1-4cfb-9275-db88297598bc.gif)  
⓶プレイ映像  
  
![VR_Test_ShugoWars_1208_スタン中の映像](https://user-images.githubusercontent.com/71632844/208294964-0cbeb53b-a4ef-49e3-bda1-b6bf1ffe66f5.gif)  
⓷手がスタン中の映像  


# 担当箇所・工夫した点
- **<ins>プレイヤー（手）のステータス関連処理</ins>**  
　姫のステータスを作成したプログラマーと連携してインターフェースを取り入れることで、ステータス変化に関する処理（ダメージを受けるなど）を共通化しコードに統一感が出るようにしました。  
　また、片手ごとに孤立したステータス・処理ができるよう列挙体で区別しインスペクターで変更できるようにすることで、左右どちらにもこの処理が対応できるようにしました。  

- **<ins>敵とプレイヤーの接触判定並びに吹っ飛ばし処理</ins>**  
　Unity既存のOculus専用アセットにハンドトラッキングを生成する処理がありますが、その中のCapsuleを生成する処理の中にタグを付与する処理を追加することでほかのオブジェクトとの接触判定を比較的簡単に取得できるようにしました。  
　また簡易的ではありますが、単に手のベクトルの方向に飛んでいくようにするだけではなく少し上方向に飛ばすように調整することで「吹っ飛ばした感」を出すようにしました。  

- **<ins>敵と敵の攻撃（ミサイル）などの共通処理の基盤作成</ins>**  
　敵の挙動に関する処理は私以外の２名がたんとうしていますが、プレイヤーに吹っ飛ばされた時の処理など敵の種類が違っても共通する処理が多々あります。そういった処理を親クラスに関数化してまとめ上げ継承させることで、子クラスでの関数呼び出しや処理入力を最低限にし敵の挙動処理を行っている２人の作業効率が良くなるようにしました。  

- **<ins>敵が倒れた際の武器のドロップ処理</ins>**  
　敵の種類と落とす武器をListの配列でインスペクターから追加や削除ができるようにすることで、敵の種類や武器の増加減少に対応できるよう設計しました。  
　また、SingletonMonoBeheviourを継承することでインスタンスを取得し武器ドロップ処理を容易にできるようしました。  

- **<ins>手（ハンドトラッキング）のセットアップフェーズ作成</ins>**  
　ハンドトラッキングのスケルトンやコライダーがうまく生成されないことがあるため、それらの対処と共に敵を倒す動作を一度試してもらえるような作りにしました。  
  
- **<ins>手の見栄えを変えるシェーダー作成</ins>**   
　既存のシェーダーを使うと見栄え的に没入感に欠ける現象が起きてしまいましたので、既存のシェーダーソースを改良し人の手に近い見た目かつプレイしやすい色合いに変更できるよう調整しました。
　また、手がスタン状態になった際に赤く点滅し視覚的にスタン状態であることがわかるようしているのですが、こちらの処理も実装しました。  
 
- **<ins>BGM・SEの2D音源処理の作成と3D音源処理の基盤づくり</ins>**   
　シングルトンパターンでの音源処理を作成しました。  
　また、3D音源は処理の基盤を作り全体に共有することで、少ない時間で実装予定の効果音をすべて実装できるようにしました。  

 

# ソースファイル
| ソースファイル | 軽い説明 | 記述・担当部分 |
| --- | --- | --- |
| ▼▼[Scriptsフォルダ](https://github.com/daichi0907/protection-WARS/tree/main/VR_ShugoWars/Assets/Scripts) |  |  |
| ▼[Behaviourフォルダ](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour) |  |  |
| [EnemyParent.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/EnemyParent.cs) | 敵の親クラス（敵共通の処理） | プレイヤーとの接触判定処理。 <br> 倒された際のアイテムのドロップ処理 。 |
| [EnemySample.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/EnemySample.cs) | 敵の子クラス（敵が実装され次第消去予定のサンプル） | 全記述 |
| [EnemyWeapon.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/EnemyWeapon.cs) | 敵の攻撃の親クラス（プレイヤーと姫との衝突判定を取得） | 全記述 |
| [HandRightCollider.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/HandRightCollider.cs) | 手の加速度を毎フレーム計算し、手の動いている向きと力を算出する | 全記述 |
| [HandTestEnemy.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/HandTestEnemy.cs) | 手のセットアップフェーズの際、試しに攻撃するための的の役割を果たす敵 | 全記述 |　
| [PlayerData.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/PlayerData.cs) | プレイヤー（片手ごと）のステータス。また、それらの状態の変更処理と状態に合わせた処理。（ダメージ処理、スタン処理など） | 「姫とライドエリアの親子関係を切る処理」以外全記述 |
| [WeaponItemBehaviour.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/WeaponItemBehaviour.cs) | 敵が落とす武器本体に関する処理 | 全記述 |
| ▼[Controllerフォルダ](https://github.com/daichi0907/protection-WARS/tree/main/VR_ShugoWars/Assets/Scripts/Controller) |  |  |
| [HandsSetUpController.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Controller/HandsSetUpController.cs) | 手のセットアップフェーズに行う処理 | 全記述 |
| [SoundController.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Controller/SoundController.cs) | ゲーム中の2D音源初期化処理、BGM関係の処理 | 姫のSEのロード処理以外記述 |
| ▼[HandSetUpフォルダ](https://github.com/daichi0907/protection-WARS/tree/main/VR_ShugoWars/Assets/Scripts/HandSetUp) | 手のセットアップフェーズで使うボタン処理がまとめてあるフォルダ |  |  |
| [HandsSetUpController.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/HandSetUp/GameStart.cs) | ゲームスタートボタンの処理 | 全記述 |
| [LeftHandOK.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/HandSetUp/LeftHandOK.cs) | 左手のセットアップ完了ボタンの処理 | 全記述 |
| [LeftHandSetReset.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/HandSetUp/LeftHandSetReset.cs) | 左手のコライダーをセットしなおすボタンの処理 | 全記述 |
| [Respawn.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/HandSetUp/Respawn.cs) | 試し打ちの敵をリスポーンするボタンの処理 | 全記述 |
| [RightHandOK.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/HandSetUp/RightHandOK.cs) | 右手のセットアップ完了ボタンの処理 | 全記述 |
| [RightHandSetReset.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/HandSetUp/RightHandSetReset.cs) | 右手のコライダーをセットしなおすボタンの処理 | 全記述 |
| ▼[Managerフォルダ](https://github.com/daichi0907/protection-WARS/tree/main/VR_ShugoWars/Assets/Scripts/Manager) |  |  |
| [WeaponManager.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Manager/WeaponManager.cs) | 敵を倒した際の武器のドロップに関する処理 | 全記述 |
| ▼[Soundフォルダ](https://github.com/daichi0907/protection-WARS/tree/main/VR_ShugoWars/Assets/Scripts/Sound) |  |  |
| [Sound2D.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Sound/Sound2D.cs) | シングルトンパターンを用いて2Dサウンドを一括管理 | 全記述 |
|  |  |  |
| ▼▼[Oculusフォルダ > VRフォルダ > Scriptsフォルダ > Utilフォルダ](https://github.com/daichi0907/protection-WARS/tree/main/VR_ShugoWars/Assets/Oculus/VR/Scripts/Util) |  |  |
| [OVRSkeleton.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Oculus/VR/Scripts/Util/OVRSkeleton.cs) | ハンドトラッキングで手の生成の際などに、Born,Bind,Capsuleを生成する処理 | Capsule生成の際タグをつける処理を追加 <br> （これにより敵との当たり判定を取得しやすくしている） |
|  |  |  |
| ▼▼[Oculusフォルダ > SampleFrameworkフォルダ > Coreフォルダ > CustomHandsフォルダ > Materialsフォルダ](https://github.com/daichi0907/protection-WARS/tree/main/VR_ShugoWars/Assets/Oculus/SampleFramework/Core/CustomHands/Materials) |  |  |
| [Hands_DiffBump.shader](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Oculus/SampleFramework/Core/CustomHands/Materials/Hands_DiffBump.shader) | ハンドトラッキングで取得した手を描画する | 元のHands＿DiffBump.shaderを改良し、白い手に発光から任意の色に全体が発光するよう修正。|

※上記に記載のないスクリプトファイルは私自身記述した部分のないスクリプトファイルとなっております。  


# 使用したデバイス・ツール
・Unity 2021.3.0f1   
・VisualStudio2019  
・Meta Quest2 **（Link接続必須のゲームとなっております。）**  
