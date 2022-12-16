# 守護WARS
3年次の後期に研究室でチーム制作を行ったゲーム。  
  
〇制作期間  
2022/09/15～2023/01/12  
（現在も制作中のため、順次更新予定です。）

〇１週間の平均作業時間  
６～８時間（授業時間〈３時間〉含む）

〇メンバー  
・プランナー　：３名  
・デザイナー　：２名  
・プログラマー：４名　（自分 + 3名）  
  
計９名  
  
(下記に担当箇所の記載あり)  

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
経験値を一定数与えることで姫のレベルが上がり自衛能力が高まります。  
回復を渡すことで姫の体力を回復することができます。  
姫に武器を渡すことで武器の種類に応じた様々な自衛をしてくれます。    
  

# 担当箇所・工夫した点
- **<ins>プレイヤー（手）のステータス関連処理</ins>**  
　姫のステータスを作成したプログラマーと連携してインターフェースを取り入れることで、ステータス変化に関する処理（ダメージを受けるなど）を共通化しコードに統一感が出るようにしました。  
　また、片手ごとに孤立したステータス・処理ができるよう列挙体で区別しインスペクターで変更できるようにすることで、左右どちらにもこの処理が対応できるようにしました。  

- **<ins>敵とプレイヤーの接触判定並びに吹っ飛ばし処理</ins>**  
　Unity既存のOculus専用アセットにハンドトラッキングを生成する処理がありますが、その中のCapsuleを生成する処理の中にタグを付与する処理を追加することでほかのオブジェクトとの接触判定を比較的簡単に取得できるようにしました。  
　また簡易的ではありますが、単に手のベクトルの方向に飛んでいくようにするだけではなく少し上方向に飛ばすように調整することで「吹っ飛ばした感」を出すようにしました。  

- **<ins>敵と敵の攻撃（ミサイル）などの共通処理の基盤作成並びにリファクタリング</ins>**  
　敵の挙動に関する処理は私以外の２名がたんとうしていますが、プレイヤーに吹っ飛ばされた時の処理など敵の種類が違っても共通する処理が多々あります。そういった処理を親クラスに関数化してまとめ上げ継承させることで、子クラスでの関数呼び出しや処理入力を最低限にし敵の挙動処理を行っている２人の作業効率が良くなるようにしました。  

- **<ins>敵が倒れた際の武器のドロップ処理</ins>**  
　敵の種類と落とす武器をListの配列でインスペクターから追加や削除ができるようにすることで、敵の種類や武器の増加減少に対応できるよう設計しました。  
　また、SingletonMonoBeheviourを継承することでインスタンスを取得し武器ドロップ処理を容易にできるようしました。  

- **<ins>手（ハンドトラッキング）のセットアップフェーズ作成</ins>**  
　ハンドトラッキングのスケルトンやコライダーがうまく生成されないことがあるため、それらの対処と共に敵を倒す動作を一度試してもらえるような作りにしました。  
 

# ソースファイル
| ソースファイル | 軽い説明 | 記述・担当部分 |
| --- | --- | --- |
| ▼▼[Scriptsフォルダ](https://github.com/daichi0907/protection-WARS/tree/main/VR_ShugoWars/Assets/Scripts) |  |  |
| ▼[Behaviourフォルダ](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour) |  |  |
| [EnemyParent.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/EnemyParent.cs) | 敵の親クラス（敵共通の処理） | プレイヤーとの接触判定処理。 <br> 倒された際のアイテムのドロップ処理 。<br>リファクタリング 。 |
| [EnemySample.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/EnemySample.cs) | 敵の子クラス（敵が実装され次第消去予定のサンプル） | 全記述 |
| [EnemyWeapon.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/EnemyWeapon.cs) | 敵の攻撃の親クラス（プレイヤーと姫との衝突判定を取得） | 全記述 |
| [HandRightCollider.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/HandRightCollider.cs) | 手の加速度を毎フレーム計算し、手の動いている向きと力を算出する | 全記述 |
| [HandTestEnemy.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/HandTestEnemy.cs) | 手のセットアップフェーズの際、試しに攻撃するための的の役割を果たす敵 | 全記述 |　
| [PlayerData.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/PlayerData.cs) | プレイヤー（片手ごと）のステータス。また、それらの状態の変更処理と状態に合わせた処理。（ダメージ処理、スタン処理など） | 全記述 |
| [WeaponItemBehaviour.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/WeaponItemBehaviour.cs) | 敵が落とす武器本体に関する処理 | 全記述 |
| ▼[Controllerフォルダ](https://github.com/daichi0907/protection-WARS/tree/main/VR_ShugoWars/Assets/Scripts/Controller) |  |  |
| [HandsSetUpController.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Behaviour/HandsSetUpController.cs) | 手のセットアップフェーズに行う処理 | 全記述 |
| ▼[HandSetUpフォルダ](https://github.com/daichi0907/protection-WARS/tree/main/VR_ShugoWars/Assets/Scripts/HandSetUp) | 手のセットアップフェーズで使うボタン処理がまとめてあるフォルダ |  |  |
| [HandsSetUpController.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/HandSetUp/GameStart.cs) | ゲームスタートボタンの処理 | 全記述 |
| [LeftHandOK.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/HandSetUp/LeftHandOK.cs) | 左手のセットアップ完了ボタンの処理 | 全記述 |
| [LeftHandSetReset.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/HandSetUp/LeftHandSetReset.cs) | 左手のコライダーをセットしなおすボタンの処理 | 全記述 |
| [Respawn.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/HandSetUp/Respawn.cs) | 試し打ちの敵をリスポーンするボタンの処理 | 全記述 |
| [RightHandOK.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/HandSetUp/RightHandOK.cs) | 右手のセットアップ完了ボタンの処理 | 全記述 |
| [RightHandSetReset.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/HandSetUp/RightHandSetReset.cs) | 右手のコライダーをセットしなおすボタンの処理 | 全記述 |
| ▼[Managerフォルダ](https://github.com/daichi0907/protection-WARS/tree/main/VR_ShugoWars/Assets/Scripts/Manager) |  |  |
| [WeaponManager.cs](https://github.com/daichi0907/protection-WARS/blob/main/VR_ShugoWars/Assets/Scripts/Manager/WeaponManager.cs) | 敵を倒した際の武器のドロップに関する処理 | 全記述 |

※上記に記載のないスクリプトファイルは私自身記述した部分のないスクリプトファイルとなっております。  


# 使用したデバイス・ツール
・Unity 2020.3.18f1   
・VisualStudio2019  
・Meta Quest2  
