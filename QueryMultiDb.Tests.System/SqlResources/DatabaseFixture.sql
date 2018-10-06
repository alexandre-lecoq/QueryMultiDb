PRINT 'Start Database Fixture SQL Script';

BEGIN TRAN;

DROP TABLE IF EXISTS TestTableOne;

CREATE TABLE TestTableOne
(
	Id INT PRIMARY KEY IDENTITY (1,1),
	Name NVARCHAR(50),
	Age INT,
	SmallText NVARCHAR(500),
	LongText NVARCHAR(4000),
	VeryLongText NVARCHAR(MAX),
	CreationDate DATETIME,
	ModificationDate DATETIME2,
	ZipCode CHAR(6),
	SmallBinary VARBINARY(100),
	LargeBinary VARBINARY(MAX),
	BigInteger BIGINT,
	Number DECIMAL(10,2),
	Boolean BIT,
	Rubis MONEY,
	Floating FLOAT(24),
	SomeDay DATE,
	SomeTime TIME,
	SomeTimeSomeWhere DATETIMEOFFSET,
	idgu UNIQUEIDENTIFIER,
	SomeXML XML,
	VersionOfRow ROWVERSION
);

INSERT INTO TestTableOne
VALUES
(
	'Shelby',
	4,
	'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse vitae tortor sollicitudin, tempus ligula ac, pretium augue. Curabitur rutrum, leo ac iaculis interdum, libero erat ullamcorper arcu, sit amet fringilla neque ante eu amet.',
	N'花间一壶酒独酌无相亲举杯邀明月对影成三人月既不解饮影徒随我身暂伴月将影行乐须及春我歌月徘徊我舞影零乱醒时同交欢醉后各分散永结无情游相期邈云汉天若不爱酒酒星不在天地若不爱酒地应无酒泉天地既爱酒爱酒不愧天已闻清比圣复道浊如贤贤圣既已饮何必求神仙三杯通大道一斗合自然但得酒中趣勿为醒者传三月咸阳城千花昼如锦谁能春独愁对此径须饮穷通与修短造化夙所禀一樽齐死生万事固难审醉后失天地兀然就孤枕不知有吾身此乐最为甚穷愁千万端美酒三百杯愁多酒虽少酒倾愁不来所以知酒圣酒酣心自开辞粟卧首阳屡空饥颜回当代不乐饮虚名安用哉蟹螯即金液糟丘是蓬莱且须饮美酒乘月醉高台',
	NULL, --VeryLongText
	'2019-11-02 20:17:15.530',
	'2023-05-21 20:17:15.8154852',
	'75001',
	0x28cd5783eb058dd916221608203cecd10b2354655f83dd472ebc520c0def36fdf944ac01519b62bd1b0e081d545d78ff65d7f6a907ead0ab44e849b7872e8ad689562f333895be43d6b25a488eb99281b12e10b73e15f9d3728ef76e4fd8d26d443de752,
	NULL, --LargeBinary
	-1628598742162015541,
	86412584.26,
	1,
	654478961.12,
	0.12154564654651,
	'1982-04-01',
	'12:42:05.564',
	'2019-11-12 20:54:04.6033333 +00:00',
	'2C100EFB-CA5C-4585-B74D-90DE35A782FF',
	'<a><b>lorem</b><c>ipsum</c></a>',
	NULL
),
(
	'Colin',
	35,
	'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque ac libero lectus. Ut non pellentesque ante, sed molestie erat. Nulla aliquet dui quis dui tincidunt ultricies. Etiam velit nisl, laoreet nec ultricies non, ullamcorper nullam.',
	N'花間一壺酒獨酌無相親舉杯邀明月對影成三人月既不解飲影徒隨我身暫伴月將影行樂須及春我歌月徘徊我舞影零亂醒時同交歡醉後各分散永結無情遊相期邈雲漢天若不愛酒酒星不在天地若不愛酒地應無酒泉天地既愛酒愛酒不愧天已聞清比聖復道濁如賢賢聖既已飲何必求神仙三杯通大道一斗合自然但得酒中趣勿為醒者傳三月咸陽城千花晝如錦誰能春獨愁對此徑須飲窮通與修短造化夙所禀一樽齊死生萬事固難審醉後失天地兀然就孤枕不知有吾身此樂最為甚窮愁千萬端美酒三百杯愁多酒雖少酒傾愁不來所以知酒聖酒酣心自開辭粟臥首陽屢空飢顏回當代不樂飲虛名安用哉蟹螯即金液糟丘是蓬萊且須飲美酒乘月醉高台',
	NULL, --VeryLongText
	'1980-01-26 10:15:45.945',
	'2049-09-12 03:48:52.5132552',
	'90210',
	0xbc9dc4,
	NULL, --LargeBinary
	98721468745321,
	-54654636.54,
	0,
	98421596.00,
	1.4546511651,
	'1947-07-26',
	'02:23:16.513',
	'2019-11-20 20:54:04.6033333 +08:00',
	'1D0E434C-B228-40A9-9A7A-7D12DF81E1EE',
	'<a><b>alea</b><c>jacta</c><d>est</d></a>',
	NULL
),
(
	'Leon',
	4561234,
	'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis ultrices arcu et fringilla vestibulum. Proin placerat lorem vitae lectus gravida porta. Fusce eu sollicitudin arcu. In eget erat in ante lacinia efficitur. Nunc vitae dui nullam.',
	N'ويستخدم مفهوم الخوارزمية أيضا في تعريف مفهوم قدرة إتخاذ القرار. هذه الفكرة هي مركزية لشرح كيفية النظام الرسمي تأتي إلى حيز الوجود بدءا من مجموعة صغيرة من البديهيات والقواعد. في المنطق، في وقت لا يمكن قياسه,الذي يتطلبه لإكمال خوارزمية كما أنه لا يرتبط على ما يبدو مع البعد المادي العرفي الذي نألفه. من هذه الشكوك، التي تميز العمل الجاري , ينبع عدم توفر تعريف الخوارزمية التي يناسب كلا من الاستخدام المحدد (بمعنى ما) والاستخدام المجرد لهذا المصطلح.',
	NULL, --VeryLongText
	'2178-06-28 12:54:32.162',
	'2245-02-27 23:02:13.4319762',
	'14100',
	0x2e252a4f0d98520b,
	NULL, --LargeBinary
	2152984132152265412,
	86432654.21,
	1,
	-44896398752.98,
	0.00000000156465,
	'1952-03-14',
	'06:32:59.123',
	'2019-11-05 20:54:04.6033333 -07:00',
	'D9DCC64C-39F0-4B19-A00F-4D8FAC8EEC22',
	'<a><b>dura</b><c>lex</c><d>sed</d><e>lex</e></a>',
	NULL
),
(
	'Kaelyn',
	654516546,
	'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam nec varius lacus. Praesent metus nisi, interdum et pharetra nec, maximus egestas turpis. Nullam vitae aliquet sapien. In hac habitasse platea dictumst. Praesent ut augue metus.',
	N'알고리즘은 자연어, 의사코드, 순서도, 프로그래밍 언어, 인터프리터가 작업하는 제어 테이블, 유한 상태 기계의 상태도 등으로 표현할 수 있다. 다음은 알고리즘 개발의 정형적인 단계이다. 알고리즘을 설계하는 기술에는 운용 과학의 방법, 설계 패턴을 이용하는 방법 등이 있다. 대부분의 알고리즘은 컴퓨터 프로그램으로 구현되지만, 전기 회로나 생물학적 신경 회로를 사용하기도 한다.',
	NULL, --VeryLongText
	'2060-08-20 01:39:17.523',
	'2078-11-05 18:45:36.9831473',
	'116000',
	0xef2241f2ef5ac333f67bd124734a906096cd59a73388e3b71377fa7f3200e092023f70e095c3a52533f3842e177df4021aab85c105856fba8b4e564cb242f0e57595de2cbbaf140ae6e3dbe80f57cf69382dc69836cdf68b2178eeac2ec020c96b4d4a8f,
	NULL, --LargeBinary
	6985412321485626652,
	87897451.78,
	0,
	2189965,
	-1.4141654165165416,
	'1956-01-15',
	'19:20:30.346',
	'2019-11-24 20:54:04.6033333 +10:30',
	'4FE9B09C-994C-4FB6-847D-5274BFCCC767',
	'<a><b>in</b><c>vino</c><d>veritas</d></a>',
	NULL
);

COMMIT TRAN;

PRINT 'Stop Database Fixture SQL Script';
