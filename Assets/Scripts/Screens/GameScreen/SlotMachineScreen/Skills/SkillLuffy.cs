using UnityEngine;
using System.Collections;

public class SkillLuffy : Skill {

	public override void Init(int level, int damage, Boss boss) {
		this.boss = boss;
	}
}
