using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabaIsYou {
    public class RuleManager {
        #region static fields

        private static RuleManager _instance = null;

        public static RuleManager Instance {
            get {
                if (_instance == null)
                    _instance = new RuleManager();
                return _instance;
            }
        }

        public RuleManager() {
            _instance = this;
        }

        #endregion // static fields





        #region private fields

        private List<Rule> activeRules = new List<Rule>(); // 현재 활성화된 규칙 목록

        #endregion // private fields





        #region public funcs

        /// <summary>
        /// 규칙 추가 ex : "BABA IS YOU"
        /// </summary>
        /// <param name="subject">"BABA"</param>
        /// <param name="verb">"IS"</param>
        /// <param name="attribute">"YOU"</param>
        public void AddRule(string subject, string verb, string attribute) {
            if (verb == "IS" && attribute == "YOU") {
                activeRules.Add(new Rule(subject, verb, "STOP"));
            }
            activeRules.Add(new Rule(subject, verb, attribute));
        }

        /// <summary>
        /// 규칙 제거 ex : "BABA IS YOU"
        /// </summary>
        /// <param name="subject">"BABA"</param>
        /// <param name="verb">"IS"</param>
        /// <param name="attribute">"YOU"</param>
        public void RemoveRule(string subject, string verb, string attribute) {
            activeRules.RemoveAll(rule => rule.Subject == subject && rule.Verb == verb && rule.Attribute == attribute);
        }

        /// <summary>
        /// 규칙 HasRule 확인 ex : "BABA IS YOU"
        /// </summary>
        /// <param name="subject">"BABA"</param>
        /// <param name="verb">"IS"</param>
        /// <param name="predicate">"YOU"</param>
        /// <returns>true / false</returns>
        public bool HasRule(string subject, string verb, string attribute) {
            Dictionary<string, string> nameMappings = new Dictionary<string, string> {
                { "#", "WALL" },
                { "B", "BABA" },
                { "O", "ROCK" }};

            if (nameMappings.ContainsKey(subject)) {
                subject = nameMappings[subject];
            }

            return activeRules.Any(rule =>
                rule.Subject == subject &&
                rule.Verb == verb &&
                rule.Attribute == attribute
            );
        }

        /// <summary>
        /// 활성화된 규칙 목록 중 "YOU" 규칙을 가진 string 목록을 반환
        /// </summary>
        /// <returns></returns>
        public List<string> GetControlledObjects() {
            List<string> controlledObjects = new List<string>();

            foreach (var rule in activeRules) {
                if (rule.Verb == "IS" && rule.Attribute == "YOU") {
                    controlledObjects.Add(rule.Subject);
                }
            }

            return controlledObjects;
        }

        public void ClearRules() {
            activeRules.Clear();
        }

        #endregion // public funcs
    }

    public class Rule {
        public string Subject { get; private set; }
        public string Verb { get; private set; }
        public string Attribute { get; private set; }

        public Rule(string subject, string verb, string attribute) {
            Subject = subject;
            Verb = verb;
            Attribute = attribute;
        }
    }
}
