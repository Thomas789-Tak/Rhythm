using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InitIngameManager //���� ���� �� �� Ŭ������ ������ �������� �ʱ�ȭ ����
{
    // �÷��̾� ����
    public static List<SoundManager.EBGM> songEquipList = new List<SoundManager.EBGM>(); // �뷡 ���� ���� Ex) 0���� ù��° ��, �ش��ϴ� ���ڰ� enum�� �ش��ϴ� ����  /// SoundManager�� �ش��ϴ� ���� ������ ��
    public static float acceleration { get; set; } // ���ӵ�
    public static float boosterMaxGauge { get; set; } //�ִ� �ν��� ������(������ �ѷ�)
    public static float boosterSpeed { get; set; } // �ν��� ���� �ӵ�
    public static float turnStrength { get; set; }
    public static float brakeForce { get; set; }
    public static float friction { get; set; }
    public static float rhythmPower { get; set; }

    // �������� ����
    public static int stagelevel { get; set; }

    // �ڵ��� ����
    public static int carType { get; set; }
}
