var MyContract = artifacts.require("StudentContract");

module.exports = function(deployer) {
  // deployment steps
  deployer.deploy(MyContract);
};
