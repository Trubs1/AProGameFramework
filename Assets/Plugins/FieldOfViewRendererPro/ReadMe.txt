// Description:��Ϸ�е�λ����Ұ�޳���ʾ
// Copyright (c) 2018 WangQiang(279980661@qq.com)
// Author:Trubs (WQ)
// Date:2018/08/08

��Ϸ�е�λ����Ұ�޳���ʾ

��Ҫ����:
  1. ͨ���䵵��ʾ������Ұ�ķ�Χ.
  2. ���ϰ���ĵط���Ұ���޳���Ⱦ.
  3. �Զ���shader,ͨ������������ͼƬ������Ⱦ����ͬ͸�����н���Ч��������.
  4. �������׶�,��չ�Ը�.�������ͽӿڷḻ,(��:��ͨ��������ɫ�ı���Ұ��ɫ,������ʾ״̬����.)
  5. ���ܿ����ɵ���,�����������Ҳ���Ὺ��̫��.
  6. �ṩ�������з���,��ѡ��Ա�.


ʹ�ò���: �������صİ���,�������Demo��������Ԥ��ʹ��.���������������Ҫ�޸���ִ�����²���.
	  1. ����MainCamera�����DrawHelper������,���󶨲���FieldOfViewRenderer.
	  2. ����ĵ�λ���۾���������������Eye�ű�(Ҳ�����ڽű��������һ��eyeTrans�ı���,�����ű����Է��ڵ�λ���ڵ�,ֻ��Ҫ��ʼ��eyeTrans����)
	  �����׿��Կ���ͼ����Ƶ

(֮ǰ�������ҵ���һ��ͨ��������޳�����Ⱦ��Ұ�ķ���,��������,���ǿ���̫��,����û���õ�������(�����20֡).�����Լ�ֱ��ͨ��OpenGL�ײ���Ⱦ��Ұ.�˷�������Ҫ����mesh,�������е���Ұͼ�ζ�һ����Ⱦ����,������mesh��Ⱦ�������ܷ�����.֮���Բ���FieldOfVisionRenderer��Ϊ�˺����ϵ�ĳ��������)


����:	2019/03/18 �����˿�ѡ���� �����FieldOfViewRenderer_andBoxSelect.unitypackage(Unity2018.3.5)

						Unity Version:2017.4.2f2  ϣ���ܰﵽ��,лл!


It is used for the enemy's vision field in the game.

Main functions:
1. Display the enemy's range of vision.
2. The vision field with obstacles won't be rendered.
3. Customize shader to render texture with gradient effect with different transparency by sampling images given by art staff.
4. The code is simple and easy to understand, with high expansibility. (for example, the view color can be changed by a given color to display the state).
5. It costs little even you set it to high quality. The performance cost can be adjusted.(It is better than some other's solution.)
6. Provide other suitable way for comparison and selection.


Use steps: Importing the package,openning the Demo scene,then you can preview it. If there are problems or need to modify, please perform the following steps.
1. Drag the DrawHelper(script) into to the MainCamera and Drag the FieldOfViewRenderer(material) into the Mat.
2. Add the Eye(script) to your unit's eye or virtual point .


Update: I add BoxSelect-module in this project at 03/18/2019 . Inporting FieldOfViewRenderer_andBoxSelect.unitypackage to see detail.

						Unity Version:2017.4.2f2  thank u very much !







