public class HostileBehaviourPeon : HostileBehaviour
{
    protected override void BuildBehaviour()
    {
        Selector attackRoutine = new Selector("AttackRoutine");
        
        Leaf intimidate = new Leaf("Intimidate", Intimidate);
        Leaf warnGroup = new Leaf("Warn Group", WarnGroup);
        Leaf getInPosition = new Leaf("Get in Melee Position", GetInPosition);
        Leaf attackPlayerMelee = new Leaf("Attack Player in Melee", AttackPlayerMelee);
        Leaf defendRetract = new Leaf("Retract from Melee Position", DefendRetract);

        Selector attackPatternStart = new Selector("Attack Pattern Start");
        Sequence attackPatternMelee = new Sequence("Attack Pattern Melee");
        Selector chooseAttackOrDefense = new Selector("Choose Attack or Defense");
        
        attackPatternStart.AddChild(warnGroup);
        attackPatternStart.AddChild(intimidate);
        
        attackPatternMelee.AddChild(getInPosition);
        chooseAttackOrDefense.AddChild(attackPlayerMelee);
        chooseAttackOrDefense.AddChild(defendRetract);
        attackPatternMelee.AddChild(chooseAttackOrDefense);
        
        attackRoutine.AddChild(attackPatternStart);
        attackRoutine.AddChild(attackPatternMelee);

        _tree.AddChild(attackRoutine);
    }
}
